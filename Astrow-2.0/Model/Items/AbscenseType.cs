namespace Astrow_2._0.Model.Items
{
    public class AbscenseType
    {
        public AbscenseType()
        {

        }
        public AbscenseType(int id, string type)
        {
            this.ID = id;
            this.Type = type;
        }

        public int ID { get; set; }
        public string Type { get; set; }
    }
}
