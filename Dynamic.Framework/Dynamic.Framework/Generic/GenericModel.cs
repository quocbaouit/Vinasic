namespace Dynamic.Framework.Generic
{
    public abstract class GenericModel
    {
        private SerializableDictionary<string, SerializableDictionary<string, string>> _modelStatus;

        public SerializableDictionary<string, SerializableDictionary<string, string>> ModelStatus
        {
            get
            {
                return this._modelStatus;
            }
        }

        protected GenericModel()
        {
            this._modelStatus = new SerializableDictionary<string, SerializableDictionary<string, string>>();
        }

        public void AddModelStatus(string fieldName, string name, string value)
        {
            if (!this.ModelStatus.ContainsKey(fieldName))
                this.ModelStatus.Add(fieldName, new SerializableDictionary<string, string>());
            this.ModelStatus[fieldName][name] = value;
        }
    }
}
