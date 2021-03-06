﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PetProduct.Configuration;
using Microsoft.Extensions.Options;
using NpgsqlTypes;
using ServiceSharedCore.Models;
using ServiceSharedCore.Utilities;

namespace ProductServiceCore.Controllers
{
    public class InventoryController : Controller
    {
        private const string SQL_GET_INVENTORY_BY_ITEM = "SELECT Item.ItemId AS id, Inventory.Qty AS Quantity FROM \"MSPETSHOP4\".Item INNER JOIN \"MSPETSHOP4\".Inventory ON Item.itemId = Inventory.itemId WHERE Item.ItemId = :itemId";
        private const string SQL_UPDATE_INVENTORY = "UPDATE \"MSPETSHOP4\".Inventory SET Qty = Qty - :quantity WHERE ItemId = :itemId";

        private string PostgreSQLConnectionString { get; set; }

        public InventoryController(IOptions<ConnectionSettings> settings)
        {
            PostgreSQLConnectionString = settings.Value.PostgreSQLConnectionString;
        }

        [HttpGet]
        [Route("inventory")]
        public ItemInfo Get(string itemId)
        {
            return DBFacilitator.GetOne<ItemInfo>(
                PostgreSQLConnectionString,
                SQL_GET_INVENTORY_BY_ITEM,
                new List<Tuple<string, string, NpgsqlDbType>>() {
                    { new Tuple<string, string, NpgsqlDbType>("itemId", itemId, NpgsqlDbType.Text) } });
        }
        [HttpPost]
        [Route("inventory")]
        public void Post([FromBody]LineItemInfo[] info)
        {
            foreach(var lineitem in info)
            {
                DBFacilitator.ExecuteCommand(
                    PostgreSQLConnectionString,
                    SQL_UPDATE_INVENTORY,
                    new List<Tuple<string, string, NpgsqlDbType>>() {
                        { new Tuple<string, string, NpgsqlDbType>("itemId", lineitem.ItemId, NpgsqlDbType.Text) },
                        { new Tuple<string, string, NpgsqlDbType>("quantity", lineitem.Quantity.ToString(), NpgsqlDbType.Integer) } }
                );
            }
        }
    }
}
