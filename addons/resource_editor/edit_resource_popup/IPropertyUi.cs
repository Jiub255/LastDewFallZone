namespace Lastdew
{
    public interface IPropertyUi
    {
        /// <summary>
        /// Use Set(StringName property, Variant value) method to set the Craftable's private properties.
        /// </summary>
        public void Save(Craftable craftable);
    }
}
