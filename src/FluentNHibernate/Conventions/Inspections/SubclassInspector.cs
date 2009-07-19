using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Conventions.Inspections
{
    public class SubclassInspector : ISubclassInspector
    {
        private readonly InspectorModelMapper<ISubclassInspector, SubclassMapping> mappedProperties = new InspectorModelMapper<ISubclassInspector, SubclassMapping>();
        private readonly SubclassMapping mapping;

        public SubclassInspector(SubclassMapping mapping)
        {
            this.mapping = mapping;
            mappedProperties.AutoMap();
            mappedProperties.Map(x => x.LazyLoad, x => x.Lazy);
        }

        public Type EntityType
        {
            get { return mapping.Type; }
        }

        public string StringIdentifierForModel
        {
            get { return mapping.Name; }
        }

        public bool IsSet(PropertyInfo property)
        {
            return mapping.IsSpecified(mappedProperties.Get(property));
        }

        public bool Abstract
        {
            get { return mapping.Abstract; }
        }

        public IEnumerable<IAnyInspector> Anys
        {
            get
            {
                return mapping.Anys
                    .Select(x => new AnyInspector(x))
                    .Cast<IAnyInspector>();
            }
        }

        public IEnumerable<ICollectionInspector> Collections
        {
            get
            {
                return mapping.Collections
                    .Select(x => new CollectionInspector(x))
                    .Cast<ICollectionInspector>();
            }
        }

        public object DiscriminatorValue
        {
            get { return mapping.DiscriminatorValue; }
        }

        public bool DynamicInsert
        {
            get { return mapping.DynamicInsert; }
        }

        public bool DynamicUpdate
        {
            get { return mapping.DynamicUpdate; }
        }

        public string Extends
        {
            get { return mapping.Extends; }
        }

        public IEnumerable<IJoinInspector> Joins
        {
            get
            {
                return mapping.Joins
                    .Select(x => new JoinInspector(x))
                    .Cast<IJoinInspector>();
            }
        }

        public Laziness LazyLoad
        {
            get { return mapping.Lazy; }
        }

        public string Name
        {
            get { return mapping.Name; }
        }

        public IEnumerable<IOneToOneInspector> OneToOnes
        {
            get
            {
                return mapping.OneToOnes
                    .Select(x => new OneToOneInspector(x))
                    .Cast<IOneToOneInspector>();
            }
        }

        public IEnumerable<IPropertyInspector> Properties
        {
            get
            {
                return mapping.Properties
                    .Select(x => new PropertyInspector(x))
                    .Cast<IPropertyInspector>();
            }
        }

        public string Proxy
        {
            get { return mapping.Proxy; }
        }

        public IEnumerable<IManyToOneInspector> References
        {
            get
            {
                return mapping.References
                    .Select(x => new ManyToOneInspector(x))
                    .Cast<IManyToOneInspector>();
            }
        }
        public bool SelectBeforeUpdate
        {
            get { return mapping.SelectBeforeUpdate; }
        }

        public IEnumerable<ISubclassInspector> Subclasses
        {
            get
            {
                return mapping.Subclasses
                    .Select(x => new SubclassInspector((SubclassMapping)x))
                    .Cast<ISubclassInspector>();
            }
        }

        public Type Type
        {
            get { return mapping.Type; }
        }
    }
}