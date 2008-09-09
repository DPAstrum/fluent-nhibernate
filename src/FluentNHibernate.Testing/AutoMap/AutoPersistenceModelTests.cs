using FluentNHibernate.AutoMap;
using FluentNHibernate.AutoMap.TestFixtures;
using FluentNHibernate.Testing.Cfg;
using NHibernate.Cfg;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMap
{
    [TestFixture]
    public class AutoPersistenceModelTests
    {
        private Configuration cfg;

        [SetUp]
        public void SetUp()
        {
            cfg = new Configuration();
            var configTester = new PersistenceConfigurationTester.ConfigTester();
            configTester.Dialect("NHibernate.Dialect.MsSql2005Dialect");
            configTester.ConfigureProperties(cfg);
        }

        [Test]
        public void TestAutoMapsIds()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleCustomColumn>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/id").Exists();
        }

        [Test]
        public void TestAutoMapsProperties()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//property").HasAttribute("name", "ExampleClassId");
        }

        [Test]
        public void TestAutoMapManyToOne()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//many-to-one").HasAttribute("name", "Parent");
        }

        [Test]
        public void TestAutoMapOneToMany()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleParentClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleParentClass>(autoMapper)
                .Element("//bag")
                .HasAttribute("name", "Examples");
        }

        [Test]
        public void TestAutoMapTimestamp()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//version")
                .HasAttribute("name", "Timestamp")
                .HasAttribute("column", "Timestamp");
        }

        [Test]
        public void TestAutoMapPropertyMergeOverridesId()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleClass>(map => map.Id(c => c.Id, "Column"));

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/id")
                .HasAttribute("name", "Id")
                .HasAttribute("column", "Column");
        }

        [Test]
        public void TestAutoMapPropertySetPrimaryKeyConvention()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .WithConvention(c=> c.GetPrimaryKeyName = p=> p.Name + "Id");

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/id")
                .HasAttribute("name", "Id")
                .HasAttribute("column", "IdId");
        }

        [Test]
        public void TestAutoMapPropertySetManyToOneKeyConvention()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .WithConvention(c =>
                                    {
                                        c.GetForeignKeyName = p => p.Name + "Id";
                                    });

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//many-to-one")
                .HasAttribute("name", "Parent")
                .HasAttribute("column", "ParentId");
        }

        [Test]
        public void TestAutoMapPropertySetOneToManyKeyConvention()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .WithConvention(c => c.GetForeignKeyNameOfParent =  t => t.Name + "Id");

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleParentClass>(autoMapper)
                .Element("//bag")
                .HasAttribute("name", "Examples")
                .Element("//key")
                .HasAttribute("column", "ExampleParentClassId");
        }

        [Test]
        public void TestAutoMapPropertySetFindPrimaryKeyConvention()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .WithConvention(c => c.FindIdentity = p => p.Name == p.DeclaringType.Name + "Id" );

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/id")
                .HasAttribute("name", "ExampleClassId")
                .HasAttribute("column", "ExampleClassId");
        }

    }
}