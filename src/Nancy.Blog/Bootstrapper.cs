using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;

namespace Nancy.Blog
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            
            container.Register<IDocumentStore>(InitializeStore());
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            var store = container.Resolve<IDocumentStore>();
            container.Register<IDocumentSession>(store.OpenSession()); 
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);

            pipelines.AfterRequest.AddItemToEndOfPipeline(ctx =>
            {
                var session = container.Resolve<IDocumentSession>();

                if (ctx.Response.StatusCode != HttpStatusCode.InternalServerError)
                {
                    session.SaveChanges();
                }

                session.Dispose();
            });
        }

        private static IDocumentStore InitializeStore()
        {
            var store = new EmbeddableDocumentStore
            {
                DataDirectory = "App_Data"
            };

            store.Initialize();

            IndexCreation.CreateIndexes(typeof(Bootstrapper).Assembly, store);
            return store;
        }
    }
}