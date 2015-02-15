using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static R4Mvc.SyntaxHelpers;

namespace R4Mvc
{
	public static class R4MvcGenerator
	{
		private static readonly string[] pramaCodes = { "1591", "3008", "3009", "0108" };

		private const string _headerText = @"
// <auto-generated />
// This file was generated by a R4Mvc.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the r4mvc.json file (i.e. the settings file), save it and rebuild.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
// 0108: suppress ""Foo hides inherited member Foo.Use the new keyword if hiding was intended."" when a controller and its abstract parent are both processed";

		public static SyntaxNode Generate(CSharpCompilation compiler, ClassDeclarationSyntax[] mvcControllerNodes)
		{
			// Create the root node and add usings, header, pragma
			var fileTree =
				CompilationUnit()
					.WithUsings("System.CodeDom.Compiler", "System.Diagnostics", "Microsoft.AspNet.Mvc")
					.WithHeader(_headerText)
					.WithPragmaCodes(false, pramaCodes);

			// controllers might be in different namespaces so we should group by namespace 
			var namespaceGroups = mvcControllerNodes.GroupBy(x=> x.Ancestors().OfType<NamespaceDeclarationSyntax>().First().Name.ToFullString());
			foreach (var namespaceControllers in namespaceGroups)
			{
				// Grab the first controller node and model and symbol for the controller's namespace
				var firstNode = namespaceControllers.First();
				var firstModel = compiler.GetSemanticModel(firstNode.SyntaxTree);
				var firstSymbol = firstModel.GetDeclaredSymbol(firstNode);
				var namespaceNode = CreateNamespace(firstSymbol.ContainingNamespace.ToString());
				
				// loop through the controllers and create a partial node for each
				foreach (var mvcControllerNode in mvcControllerNodes)
				{
					var model = compiler.GetSemanticModel(mvcControllerNode.SyntaxTree);
					var mvcSymbol = model.GetDeclaredSymbol(mvcControllerNode);

					// build controller partial class node 
					// add a default constructor if there are some but none are zero length
					var genControllerClass = CreateClass(
						mvcSymbol.Name,
						mvcControllerNode.TypeParameterList?.Parameters.ToArray(),
						SyntaxKind.PublicKeyword,
						SyntaxKind.PartialKeyword);

					if (!mvcSymbol.Constructors.IsEmpty || !mvcSymbol.Constructors.Any(x => x.Parameters.Length == 0))
					{
						genControllerClass = genControllerClass.WithDefaultConstructor(true, SyntaxKind.PublicKeyword);
					}

					// add all method stubs, TODO criteria for this: only public virtual actionresults?
					// add subclasses, fields, properties, constants for action names
					genControllerClass = genControllerClass
						.WithMethods(mvcSymbol)
						.WithStringField("Name", mvcControllerNode.Identifier.ToString(), true, SyntaxKind.PublicKeyword, SyntaxKind.ReadOnlyKeyword)
						.WithStringField("NameConst", mvcControllerNode.Identifier.ToString(), true, SyntaxKind.PublicKeyword, SyntaxKind.ConstKeyword)
						.WithStringField("Area", mvcControllerNode.Identifier.ToString(), true, SyntaxKind.PublicKeyword, SyntaxKind.ReadOnlyKeyword)
						.WithField("s_actions", "ActionNamesClass", SyntaxKind.StaticKeyword, SyntaxKind.ReadOnlyKeyword)
						.WithActionNameClass(mvcControllerNode)
						.WithActionConstantsClass(mvcControllerNode)
						.WithViewNamesClass();

					namespaceNode = namespaceNode.AddMembers(genControllerClass);

					// create R4MVC_[Controller] class inheriting from partial
					var r4ControllerClass = CreateClass(
						GetR4MVCControllerClassName(genControllerClass),
						null,
						SyntaxKind.PublicKeyword,
						SyntaxKind.PartialKeyword)
						.WithAttributes(CreateGeneratedCodeAttribute(), CreateDebugNonUserCodeAttribute())
						.WithBaseTypes(mvcControllerNode.ToQualifiedName())
						.WithDefaultConstructor(false, SyntaxKind.PublicKeyword);

					namespaceNode = namespaceNode.AddMembers(r4ControllerClass);

				}

				fileTree = fileTree.AddMembers(namespaceNode);
			}

			// add the dummy class using in the derived controller partial class
			var r4Namespace = CreateNamespace("R4MVC");
			r4Namespace = r4Namespace.WithDummyClass();
			fileTree = fileTree.AddMembers(r4Namespace);
			
			// create static MVC class and add controller fields 
			// TODO get 'MVC' static class name from overridable config, 'MVC' by default
			var mvcStaticClass =
				CreateClass("MVC", null, SyntaxKind.PublicKeyword, SyntaxKind.StaticKeyword, SyntaxKind.PartialKeyword)
					.WithAttributes(CreateGeneratedCodeAttribute(), CreateDebugNonUserCodeAttribute())
					.WithControllerFields(mvcControllerNodes);
			fileTree = fileTree.AddMembers(mvcStaticClass);

			// create static Links class (scripts, content, bundles?)
			var linksNamespace = CreateNamespace("Links");
			// TODO will default to look in the wwwroot of the project but need to get this from config
			var staticFolders = new[] { "css", "lib" };
			foreach (var folder in staticFolders)
			{
				var staticFolderClass =
					CreateClass(folder, null, SyntaxKind.PublicKeyword, SyntaxKind.StaticKeyword)
						.WithAttributes(CreateGeneratedCodeAttribute(), CreateDebugNonUserCodeAttribute())
						.WithStaticFieldsForFiles()
						.WithUrlMethods()
						.WithStringField("URLPATH", "~/" + folder, false, SyntaxKind.PrivateKeyword, SyntaxKind.ConstKeyword);

				linksNamespace = linksNamespace.AddMembers(staticFolderClass);
			}
			fileTree = fileTree.AddMembers(linksNamespace);

			// TODO create R4MVCHelpers class


			fileTree = fileTree.NormalizeWhitespace();
			// reenable pragma codes after last node
			// BUG NormalizeWhitespace is messing up prama (called when writing file)
			fileTree = fileTree.WithPragmaCodes(true, pramaCodes);

			return fileTree;
		}

		private static string GetR4MVCControllerClassName(ClassDeclarationSyntax genControllerClass)
		{
			return string.Format("R4MVC_{0}", genControllerClass.Identifier);
		}

		public static ClassDeclarationSyntax WithActionNameClass(this ClassDeclarationSyntax node, ClassDeclarationSyntax controllerNode)
		{
			// create ActionNames sub class using symbol method names
			return node.WithSubClassMembersAsStrings(
				controllerNode,
				"ActionNamesClass",
				SyntaxKind.PublicKeyword,
				SyntaxKind.ReadOnlyKeyword);
		}

		public static ClassDeclarationSyntax WithActionConstantsClass(this ClassDeclarationSyntax node, ClassDeclarationSyntax controllerNode)
		{
			// create ActionConstants sub class
			return node.WithSubClassMembersAsStrings(
				controllerNode,
				"ActionNameConstants",
				SyntaxKind.PublicKeyword,
				SyntaxKind.ConstKeyword);
		}

		public static ClassDeclarationSyntax WithViewNamesClass(this ClassDeclarationSyntax node)
		{
			// TODO create subclass called ViewsClass
			// TODO figure out method of view discovery
			// TODO create ViewNames get property returning static instance of ViewNames subclass
				// TODO create subclass in ViewsClass called ViewNames 
					// TODO create string field per view
			// TODO create string field per view of relative url
			return node;
		}

		public static ClassDeclarationSyntax WithControllerFields(this ClassDeclarationSyntax node, ClassDeclarationSyntax[] mvcControllerNodes)
		{
			// TODO field name should be overriddable via config, stripping off 'controller' by default
			// TODO add extension method to customise field initializer as this needs to be the one returned from GetR4MVCControllerClassName
			return node.AddMembers(
				mvcControllerNodes.Select(
					x => CreateFieldWithDefaultInitializer(
						x.Identifier.ToString().Replace("Controller", string.Empty),
						x.ToQualifiedName(),
						SyntaxKind.PublicKeyword,
						SyntaxKind.StaticKeyword)).Cast<MemberDeclarationSyntax>().ToArray()); 
		}

		public static ClassDeclarationSyntax WithStaticFieldsForFiles(this ClassDeclarationSyntax node)
		{
			// TODO add string field for each file
			// TODO add check for IsProduction
			return node;
		}

		public static ClassDeclarationSyntax WithUrlMethods(this ClassDeclarationSyntax node)
		{
			// TODO add url methods that call delegated virtual path provider
			return node;
		}


		public static NamespaceDeclarationSyntax WithDummyClass(this NamespaceDeclarationSyntax node)
		{
			const string dummyClassName = "Dummy";
			var dummyClass =
				CreateClass(dummyClassName)
					.WithModifiers(SyntaxKind.PublicKeyword)
					.WithAttributes(CreateGeneratedCodeAttribute(), CreateDebugNonUserCodeAttribute())
					.WithDefaultConstructor(false, SyntaxKind.PrivateKeyword)
					.WithField("Instance", dummyClassName, SyntaxKind.PublicKeyword, SyntaxKind.StaticKeyword);

			return node.AddMembers(dummyClass);
		}
	}
}