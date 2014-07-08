namespace WebDemo {
    using Nancy;
    using Nancy.Session;
    using Nancy.ViewEngines.Razor;
    using System.Collections.Generic;

    public class Bootstrapper : DefaultNancyBootstrapper {
        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper

        protected override void ApplicationStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines) {
            base.ApplicationStartup(container, pipelines);

            StaticConfiguration.DisableErrorTraces = false;
            CookieBasedSessions.Enable(pipelines);
        }
    }

    public class MyRazorConfiguration : IRazorConfiguration {
        public bool AutoIncludeModelNamespace {
            get { return true; }
        }

        public IEnumerable<string> GetAssemblyNames() {
            yield return "System.Runtime, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
            yield return "figo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        }

        public IEnumerable<string> GetDefaultNamespaces() {
            yield return "System.Collections.Generic";
            yield return "System.Linq";
        }
    }
}