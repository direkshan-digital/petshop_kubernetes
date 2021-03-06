using Newtonsoft.Json;
using PetShop.IDAL;
using PetShop.Model;
using System.Configuration;

namespace PetShop.BLL {

    /// <summary>
    /// A business component to manage the inventory management for an item
    /// </summary>
    public class Inventory {

        // Get an instance of the Inventory DAL using the DALFactory
        // Making this static will cache the DAL instance after the initial load
        //private static readonly IInventory dal = PetShop.DALFactory.DataAccess.CreateInventory();

        /// <summary>
        /// A method to get the current quanity in stock for an individual item
        /// </summary>
        /// <param name="itemId">A unique identifier for an item</param>
        /// <returns>Current quantity in stock</returns>
        public int CurrentQuantityInStock(string itemId) {

            // Validate input
            if (string.IsNullOrEmpty(itemId.Trim()))
                return 0;

            var requestURL = ConfigurationManager.AppSettings["ServicesBaseURL"] + "inventory";
            var result = Utilities.WebAPICall(requestURL);
            ItemInfo info = JsonConvert.DeserializeObject<ItemInfo>(result);
            return info.Quantity;
        }

        /// <summary>
        /// Reduce the current quantity in stock for an order's lineitems
        /// </summary>
        /// <param name="items">An array of order line items</param>
        public void TakeStock(LineItemInfo[] items)
        {

            // Reduce the stock level in the data store
            var requestURL = ConfigurationManager.AppSettings["ServicesBaseURL"] + "inventory";
            var result = Utilities.WebAPICallWithData(requestURL, JsonConvert.SerializeObject(items));
        }
    }
}
