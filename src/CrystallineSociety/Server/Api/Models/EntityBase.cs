using System.ComponentModel;

using Bit.Model.Contracts;
using MelkRadar.Core.Model.Util;

namespace CrystallineSociety.Server.Api.Models
{
    // ToDo: check if INotifyPropertyChanged is needed
    public abstract class EntityBase : IEntity, IEquatable<EntityBase>, INotifyPropertyChanging

    {
        protected EntityBase()
        {

        }

        protected EntityBase(bool initialize)
        {
            if (initialize)
                Initialize();
        }

        public virtual Guid Id { get; set; }

        private void Initialize()
        {
            Id = Guid.NewGuid();
            OnInitialize();
        }

        protected virtual void OnInitialize()
        {
        }

        public bool Equals(EntityBase other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EntityBase)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetDeterministicHashCode();
        }


        public event PropertyChangingEventHandler PropertyChanging;

        public void OnPropertyChanged(string propertyName, object before, object after)
        {
            if (propertyName.EndsWith("DateTime"))
            {
                var currentProp = GetType().GetProperty(propertyName);

                if (currentProp.PropertyType == typeof(DateTimeOffset?) || currentProp.PropertyType == typeof(DateTimeOffset))
                {
                    var equivalentDateOnlyPropName = propertyName.Replace("DateTime", "Date");
                    var equivalentDateOnlyProp = GetType().GetProperty(equivalentDateOnlyPropName);
                    if (equivalentDateOnlyProp != null && (equivalentDateOnlyProp.PropertyType == typeof(DateTime?) || equivalentDateOnlyProp.PropertyType == typeof(DateTime)))
                    {
                        equivalentDateOnlyProp.SetValue(this, after is DateTimeOffset dateTimeOffset ? dateTimeOffset.Date : (DateTime?)null);
                    }
                }
            }
        }

    }
}
