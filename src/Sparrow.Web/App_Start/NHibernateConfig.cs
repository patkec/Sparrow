using System;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Sparrow.Domain.Models;

namespace Sparrow.Web.App_Start
{
    public class NHibernateConfig
    {
        public static ISessionFactory BuildSessionFactory()
        {
            var configuration = Fluently
                .Configure()
                .Database(
                    MsSqlConfiguration.MsSql2008
                        .ConnectionString(str => str.FromConnectionStringWithKey("sparrow"))
                        .ShowSql
                )
                .Mappings(m =>
                {
                    m.AutoMappings
                        .Add(
                            AutoMap.AssemblyOf<User>(new AutoMapConfiguration())
                                .IgnoreBase<EntityBase>()
                                .Conventions.Add(
                                    new NotNullConvention(),
                                    ConventionBuilder.Class.Always(c => c.Schema("dbo")),
                                    ConventionBuilder.Id.Always(id => id.GeneratedBy.GuidComb()),
                                    ConventionBuilder.HasMany.Always(many => many.Cascade.AllDeleteOrphan()),
                                    DefaultCascade.All(),
                                    PrimaryKey.Name.Is(id => "ID"),
                                    ForeignKey.EndsWith("ID"),
                                    DefaultAccess.CamelCaseField(CamelCasePrefix.Underscore)
                                )
                        );

                })
                .BuildConfiguration();

            // Uncomment to automatically generate database tables.
            // new SchemaExport(configuration).Create(false, true);

            return configuration.BuildSessionFactory();
        }

        private class NotNullConvention : IPropertyConvention, IPropertyConventionAcceptance
        {
            public void Apply(IPropertyInstance instance)
            {
                instance.Not.Nullable();
            }

            public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
            {
                criteria.Expect(x => x.Nullable && (x.Name == "Name" || x.Name == "Title"));
            }
        }

        private class AutoMapConfiguration : DefaultAutomappingConfiguration
        {
            public override bool ShouldMap(Member member)
            {
                Member backingField;
                return base.ShouldMap(member) && member.TryGetBackingField(out backingField);
            }

            public override bool ShouldMap(Type type)
            {
                return type.IsSubclassOf(typeof(EntityBase));
            }
        }
    }
}