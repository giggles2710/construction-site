using System.Web;
using System.Web.Optimization;

namespace _170516
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui.js",
                        "~/Scripts/jquery.cslider.js",
                        "~/Scripts/inputmask.js",
                        "~/Scripts/inputmask.date.extensions.js",
                        "~/Scripts/inputmask.numeric.extensions.js",
                        "~/Scripts/inputmask.phone.extensions.js",
                        "~/Scripts/inputmask.regex.extensions.js",
                        "~/Scripts/inputmask.extensions.js",
                        "~/Scripts/jquery.inputmask.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-select.js",
                      "~/Scripts/bootstrap-collapse.js",
                      "~/Scripts/bootstrap-transaction.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery_upload").Include(
                      "~/Scripts/jquery-upload/vendor/jquery.ui.widget.js",
                      "~/Scripts/jquery-upload/vendor/load-image.all.min.js",
                      "~/Scripts/jquery-upload/vendor/canvas-to-blob.min.js",
                      "~/Scripts/jquery-upload/jquery.iframe-transport.js",
                      "~/Scripts/jquery-upload/jquery.fileupload.js",
                      "~/Scripts/jquery-upload/jquery.fileupload-process.js",
                      "~/Scripts/jquery-upload/jquery.fileupload-image.js",
                      "~/Scripts/jquery-upload/jquery.fileupload-audio.js",
                      "~/Scripts/jquery-upload/jquery.fileupload-video.js",
                      "~/Scripts/jquery-upload/jquery.fileupload-validate.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin_script").Include(
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/bootstrap-select.js",
                        "~/Scripts/jquery.dataTables.js",
                        "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/main_script").Include(
                        "~/Scripts/Common/main.js",
                        "~/Scripts/dlMenu/custom.dlmenu.js",
                        "~/Scripts/dlMenu/modernizr.custom.js",
                        "~/Scripts/tinymce/tinymce.min.js",
                        "~/Scripts/slick.min.js",
                        "~/Scripts/owl.carousel.js",
                        "~/Scripts/bootstrap-collapse.js",
                        "~/Scripts/bootstrap-transition.js",
                        "~/Scripts/bootbox.min.js",
                        "~/Scripts/toastr.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/dlMenu/css/component.css",
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-select/bootstrap-select.css",
                      "~/Content/font-awesome/css/font-awesome.css",
                      "~/Content/common.css",
                      "~/Content/slick.css",
                      "~/Content/slick-theme.css",
                      "~/Content/owl.carousel/owl.carousel.css",
                      "~/Content/owl.carousel/owl.theme.css",
                      "~/Content/owl.carousel/owl.transitions.css",
                      "~/Content/toastr.css"));

            bundles.Add(new StyleBundle("~/Content/admin_css").Include(
                      "~/Content/Admin/bootstrap/css/bootstrap.css",
                      "~/Content/bootstrap-select/bootstrap-select.css",
                      "~/Content/font-awesome/css/font-awesome.css",
                      "~/Content/admin.common.css",
                      "~/Content/jquery-ui.css",
                       "~/Content/toastr.css"));
        }
    }
}
