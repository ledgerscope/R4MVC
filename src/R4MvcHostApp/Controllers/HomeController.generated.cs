// <auto-generated />
// This file was generated by a R4Mvc.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the r4mvc.json file (i.e. the settings file), save it and rebuild.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
// 0108: suppress "Foo hides inherited member Foo.Use the new keyword if hiding was intended." when a controller and its abstract parent are both processed
#pragma warning disable 1591, 3008, 3009, 0108
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using R4Mvc;

namespace R4MvcHostApp.Controllers
{
    public partial class HomeController
    {
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public HomeController()
        {
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        protected HomeController(Dummy d)
        {
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(IActionResult result)
        {
            var callInfo = result.GetR4MvcResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<IActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(IActionResult result)
        {
            var callInfo = result.GetR4MvcResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<IActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [GeneratedCode("R4Mvc", "1.0")]
        public HomeController Actions => MVC.Home;
        [GeneratedCode("R4Mvc", "1.0")]
        public readonly string Area = "";
        [GeneratedCode("R4Mvc", "1.0")]
        public readonly string Name = "Home";
        [GeneratedCode("R4Mvc", "1.0")]
        public const string NameConst = "Home";
        [GeneratedCode("R4Mvc", "1.0")]
        static readonly ActionNamesClass s_ActionNames = new ActionNamesClass();
        [GeneratedCode("R4Mvc", "1.0")]
        public ActionNamesClass ActionNames => s_ActionNames;
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Index = "Index";
            public readonly string About = "About";
            public readonly string Contact = "Contact";
            public readonly string Error = "Error";
            public readonly string ExtensionTest = "ExtensionTest";
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
            public const string About = "About";
            public const string Contact = "Contact";
            public const string Error = "Error";
            public const string ExtensionTest = "ExtensionTest";
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            [GeneratedCode("R4Mvc", "1.0")]
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            [GeneratedCode("R4Mvc", "1.0")]
            public _ViewNamesClass ViewNames => s_ViewNames;
            public class _ViewNamesClass
            {
                public readonly string About = "About";
                public readonly string Contact = "Contact";
                public readonly string Index = "Index";
            }

            public string About = "~/Views/Home/About.cshtml";
            public string Contact = "~/Views/Home/Contact.cshtml";
            public string Index = "~/Views/Home/Index.cshtml";
            [GeneratedCode("R4Mvc", "1.0")]
            static readonly _DisplayTemplatesClass s_DisplayTemplates = new _DisplayTemplatesClass();
            [GeneratedCode("R4Mvc", "1.0")]
            public _DisplayTemplatesClass DisplayTemplates => s_DisplayTemplates;
            [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
            public partial class _DisplayTemplatesClass
            {
                public readonly string Test = "Test";
            }

            [GeneratedCode("R4Mvc", "1.0")]
            static readonly _EditorTemplatesClass s_EditorTemplates = new _EditorTemplatesClass();
            [GeneratedCode("R4Mvc", "1.0")]
            public _EditorTemplatesClass EditorTemplates => s_EditorTemplates;
            [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
            public partial class _EditorTemplatesClass
            {
                public readonly string Test = "Test";
            }
        }

        [GeneratedCode("R4Mvc", "1.0")]
        static readonly ViewsClass s_Views = new ViewsClass();
        [GeneratedCode("R4Mvc", "1.0")]
        public ViewsClass Views => s_Views;
    }

    [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
    public partial class R4MVC_HomeController : R4MvcHostApp.Controllers.HomeController
    {
        public R4MVC_HomeController(): base (Dummy.Instance)
        {
        }

        [NonAction]
        partial void IndexOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo);
        [NonAction]
        public override Microsoft.AspNetCore.Mvc.IActionResult Index()
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.Index);
            IndexOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void AboutOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo);
        [NonAction]
        public override Microsoft.AspNetCore.Mvc.IActionResult About()
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.About);
            AboutOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void ContactOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo);
        [NonAction]
        public override Microsoft.AspNetCore.Mvc.IActionResult Contact()
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.Contact);
            ContactOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void ErrorOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo);
        [NonAction]
        public override Microsoft.AspNetCore.Mvc.IActionResult Error()
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.Error);
            ErrorOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void ExtensionTestOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo);
        [NonAction]
        public override Microsoft.AspNetCore.Mvc.IActionResult ExtensionTest()
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.ExtensionTest);
            ExtensionTestOverride(callInfo);
            return callInfo;
        }
    }
}
#pragma warning restore 1591, 3008, 3009, 0108
