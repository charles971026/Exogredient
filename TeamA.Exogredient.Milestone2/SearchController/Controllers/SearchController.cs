﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TeamA.Exogredient.DAL;
using TeamA.Exogredient.DataHelpers;
using TeamA.Exogredient.Managers;
using TeamA.Exogredient.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SearchController.Controllers
{
    [Route("api/[controller]")]
    public class SearchController : Controller
    {

        //[EnableCors]
        //[HttpGet("byIngredient")]
        //public async Task<IEnumerable<StoreResult>> GetStoresByIngredientNameAsync(string searchTerm, double latitude, double longitude,
        //    double radius, int pagination, int failureCount, string username, string ipAddress)
        //{
        //    Console.WriteLine(radius);
        //    var connection = "server=localhost;user=root;database=exogredient;port=3306;password=1234567890";
        //    var map = "server=localhost;user=root;database=mapping_table;port=3306;password=1234567890";
        //    var log = "server=localhost;user=root;database=exogredient_log;port=3306;password=1234567890";
        //    StoreDAO storeDao = new StoreDAO(connection);
        //    UploadDAO uploadDAO = new UploadDAO(connection);
        //    var logdao = new LogDAO(log);
        //    var searchService = new SearchService(storeDao, uploadDAO);
        //    var mapdao = new MapDAO(map);
        //    var mask = new MaskingService(mapdao);
        //    var ffLogging = new FlatFileLoggingService(mask);
        //    var dsLogging = new DataStoreLoggingService(logdao, mask);
        //    var loggingManager = new LoggingManager(ffLogging, dsLogging);
        //    string enUSAff = "en_US/en_US.aff";
        //    string enUSDic = "en_US/en_US.dic";
        //    SearchManager searchManager = new SearchManager(searchService, loggingManager, enUSDic, enUSAff);

        //    var result = await searchManager.GetStoresByIngredientNameAsync(searchTerm, latitude, longitude, radius, pagination, failureCount, username, ipAddress).ConfigureAwait(false);
        //    var stores = result.Data;
        //    return stores;
        //}

        //[EnableCors]
        //[HttpGet("byStore")]
        //public async Task<IEnumerable<StoreResult>> GetStoresByStoreNameAsync(string searchTerm, double latitude, double longitude,
        //    double radius, int pagination, int failureCount, string username, string ipAddress)
        //{
        //    Console.WriteLine(radius);
        //    var connection = "server=localhost;user=root;database=exogredient;port=3306;password=1234567890";
        //    var map = "server=localhost;user=root;database=mapping_table;port=3306;password=1234567890";
        //    var log = "server=localhost;user=root;database=exogredient_log;port=3306;password=1234567890";
        //    StoreDAO storeDao = new StoreDAO(connection);
        //    UploadDAO uploadDAO = new UploadDAO(connection);
        //    var logdao = new LogDAO(log);
        //    var searchService = new SearchService(storeDao, uploadDAO);
        //    var mapdao = new MapDAO(map);
        //    var mask = new MaskingService(mapdao);
        //    var ffLogging = new FlatFileLoggingService(mask);
        //    var dsLogging = new DataStoreLoggingService(logdao, mask);
        //    var loggingManager = new LoggingManager(ffLogging, dsLogging);
        //    string enUSAff = "en_US/en_US.aff";
        //    string enUSDic = "en_US/en_US.dic";
        //    SearchManager searchManager = new SearchManager(searchService, loggingManager, enUSDic, enUSAff);

        //    var result = await searchManager.GetStoresByStoreNameAsync(searchTerm, latitude, longitude, radius, pagination, failureCount, username, ipAddress).ConfigureAwait(false);
        //    var stores = result.Data;
        //    return stores;
        //}

        [EnableCors]
        [HttpGet("getTotalNum")]
        public async Task<int> GetTotalResultsNumberAsync(string searchTerm, double latitude, double longitude, double radius, string searchBy,
                                                          int failureCount, string username, string ipAddress)
        {
            var connection = "server=localhost;user=root;database=exogredient;port=3306;password=1234567890";
            var map = "server=localhost;user=root;database=mapping_table;port=3306;password=1234567890";
            var log = "server=localhost;user=root;database=exogredient_log;port=3306;password=1234567890";
            StoreDAO storeDao = new StoreDAO(connection);
            UploadDAO uploadDAO = new UploadDAO(connection);
            var logdao = new LogDAO(log);
            var searchService = new SearchService(storeDao, uploadDAO);
            var mapdao = new MapDAO(map);
            var mask = new MaskingService(mapdao);
            var ffLogging = new FlatFileLoggingService(mask);
            var dsLogging = new DataStoreLoggingService(logdao, mask);
            var loggingManager = new LoggingManager(ffLogging, dsLogging);
            string enUSAff = "en_US/en_US.aff";
            string enUSDic = "en_US/en_US.dic";
            SearchManager searchManager = new SearchManager(searchService, loggingManager, enUSDic, enUSAff);

            var result = await searchManager.GetTotalResultsNumberAsync(searchTerm, latitude, longitude, radius, searchBy, failureCount, username, ipAddress);
            var num = result.Data;
            Console.WriteLine($"total num: {num}");
            return num;
        }

        [EnableCors]
        [HttpGet("getResults")]
        public async Task<IEnumerable<StoreResult>> GetStoresByStoreNameAsync(string searchTerm, double latitude, double longitude, double radius, string searchBy,
            double lastStoreData, int lastStoreId, string sortOption, bool fromSmallest, int failureCount, string username, string ipAddress)
        {
            Console.WriteLine(searchBy);
            Console.WriteLine(fromSmallest);

            var connection = "server=localhost;user=root;database=exogredient;port=3306;password=1234567890";
            var map = "server=localhost;user=root;database=mapping_table;port=3306;password=1234567890";
            var log = "server=localhost;user=root;database=exogredient_log;port=3306;password=1234567890";
            StoreDAO storeDao = new StoreDAO(connection);
            UploadDAO uploadDAO = new UploadDAO(connection);
            var logdao = new LogDAO(log);
            var searchService = new SearchService(storeDao, uploadDAO);
            var mapdao = new MapDAO(map);
            var mask = new MaskingService(mapdao);
            var ffLogging = new FlatFileLoggingService(mask);
            var dsLogging = new DataStoreLoggingService(logdao, mask);
            var loggingManager = new LoggingManager(ffLogging, dsLogging);
            string enUSAff = "en_US/en_US.aff";
            string enUSDic = "en_US/en_US.dic";
            SearchManager searchManager = new SearchManager(searchService, loggingManager, enUSDic, enUSAff);

            var result = await searchManager.GetStoresAsync(searchTerm, latitude, longitude, radius,searchBy,lastStoreData,lastStoreId,sortOption,fromSmallest, failureCount, username, ipAddress).ConfigureAwait(false);
            var stores = result.Data;
            return stores;
        }

        [EnableCors]
        [HttpGet("storeView")]
        public async Task<Tuple<StoreViewData,IEnumerable<IngredientResult>>> GetIngredientsAsync(string username, string ipAddress, int failureCount, int storeId, string lastIngredientName = null, string ingredientName = null)
        {
            Console.WriteLine(username);
            Console.WriteLine(ipAddress);
            Console.WriteLine(failureCount);
            Console.WriteLine(storeId);
            Console.WriteLine(lastIngredientName);
            Console.WriteLine(ingredientName);

            var connection = "server=localhost;user=root;database=exogredient;port=3306;password=1234567890";
            var map = "server=localhost;user=root;database=mapping_table;port=3306;password=1234567890";
            var log = "server=localhost;user=root;database=exogredient_log;port=3306;password=1234567890";
            StoreDAO storeDao = new StoreDAO(connection);
            UploadDAO uploadDAO = new UploadDAO(connection);
            var logdao = new LogDAO(log);
            var searchService = new SearchService(storeDao, uploadDAO);
            var mapdao = new MapDAO(map);
            var mask = new MaskingService(mapdao);
            var ffLogging = new FlatFileLoggingService(mask);
            var dsLogging = new DataStoreLoggingService(logdao, mask);
            var loggingManager = new LoggingManager(ffLogging, dsLogging);
            string enUSAff = "en_US/en_US.aff";
            string enUSDic = "en_US/en_US.dic";
            SearchManager searchManager = new SearchManager(searchService, loggingManager, enUSDic, enUSAff);


            var ingredientsResult = await searchManager.GetIngredientsAsync(username, ipAddress, failureCount, storeId, lastIngredientName, ingredientName).ConfigureAwait(false);
            var ingredients = ingredientsResult.Data;
            var storeViewDataResult = await searchManager.GetStoreViewDataAsync(storeId, failureCount);
            var storeViewData = storeViewDataResult.Data;

            var tuple = new Tuple<StoreViewData, IEnumerable<IngredientResult>>(storeViewData, ingredients);
            return tuple;
        }

        [EnableCors]
        [HttpGet("image")]
        public IActionResult GetImage(int id)
        {
            Console.WriteLine("Image retrieved");
            var image = System.IO.File.OpenRead($"images/store{id}.jpg");
            return File(image, "image/jpeg");

        }

    }
}
