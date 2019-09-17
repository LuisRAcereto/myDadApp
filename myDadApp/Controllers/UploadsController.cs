using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using myDadApp.Models;
using Newtonsoft.Json;

namespace myDadApp.Controllers
{
    public class UploadsController : Controller
    {
        private readonly CloudTable _table;
        private readonly CloudBlobContainer _blob;
        private readonly CloudQueue _queue;
        private readonly string _partitionKey = "NewImage";

        public UploadsController(CloudTable table, CloudBlobContainer blob, CloudQueue queue)
        {
            _table = table;
            _blob = blob;
            _queue = queue;
        }


        // GET: Uploads
        public async Task<ActionResult> Index()
        {
            return View(await GetTableItems(_partitionKey));
        }


        private async Task<List<Upload>> GetTableItems(string partitionKey)
        {
            List<Upload> myData = new List<Upload>();

            var myQuery = new TableQuery<Upload>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey",
                                    QueryComparisons.Equal, partitionKey));
            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<Upload> rcSegment = await _table.ExecuteQuerySegmentedAsync(myQuery, token);
                token = rcSegment.ContinuationToken;

                foreach (var item in rcSegment)
                {
                    myData.Add(item);

                }
            } while (token != null);
            return myData;
        }

        public async Task<Upload> GetItem(string partitionKey, string rowKey)
        {
            TableOperation operation = TableOperation.Retrieve<Upload>(partitionKey, rowKey);

            TableResult result = await _table.ExecuteAsync(operation);

            return (Upload)(dynamic)result.Result;
        }

        // GET: Uploads/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Uploads/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Uri,CreateDt,IsCat,IsDog,Thumbnail,PartitionKey,RowKey,Timestamp,ETag")]
            Upload upload, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                // MB: Upload Blob
                foreach (var item in Request.Form.Files)
                {
                    // upload the file!
                    upload.Name = item.FileName;

                    var myBlob = _blob.GetBlockBlobReference(item.FileName); //, new DateTimeOffset(new DateTime(100000000)));
                    await _blob.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

                    using (var stream = item.OpenReadStream())
                    {
                        await myBlob.UploadFromStreamAsync(stream);
                    };
                    upload.URI = myBlob.Uri.ToString();
                }
                // MB: Add Table Item
                TableOperation azInsert = TableOperation.InsertOrReplace(upload);
                await _table.ExecuteAsync(azInsert);

                // MB: Add work to Queue
                await _queue.AddMessageAsync(new CloudQueueMessage(JsonConvert.SerializeObject(upload)));

                return RedirectToAction(nameof(Index));
            }
            return View(upload);
        }

        // GET: Uploads/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Uploads/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Uploads/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Uploads/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}