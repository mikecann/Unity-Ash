namespace Net.RichardLord.Ash.Core
{
    public class Node
    {
        /// <summary>
        /// The entity whose components are included in the node.
        /// </summary>
        public EntityBase Entity { get; set; }      

        /// <summary>
        /// Used by the NodeList class. The previous node in a node list.
        /// </summary>
        public Node Previous { get; set; }

        /// <summary>
        /// Used by the NodeList class. The next node in a node list.
        /// </summary>
        public Node Next { get; set; }

        public object GetProperty(string propertyName)
        {
            return GetType().GetProperty(propertyName).GetValue(this, null);
        }

        public void SetProperty(string propertyName, object value)
        {
            GetType().GetProperty(propertyName).SetValue(this, value, null);
        }
    }
}
