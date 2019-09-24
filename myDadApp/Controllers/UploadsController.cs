using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using myDadApp.Models;

namespace myDadApp.Controllers
{
    public class UploadsController : Controller
    {
        private readonly CloudBlobContainer _blob;
        private readonly CloudTable _table;

        public UploadsController(CloudBlobContainer blobContainer, CloudTable cloudTable)
        {
            _blob = blobContainer;
            _table = cloudTable;
        }
        // GET: Uploads
        public async Task<ActionResult> Index()
        {
            return View(await GetTableItems("Demos"));
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

        // POST: Uploads/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection, List<IFormFile> files)
        {
            try
            {
                // TODO: Add insert logic here
                foreach(var item in Request.Form.Files)
                {
                    var myBlob = _blob.GetBlockBlobReference(item.FileName);
                    await _blob.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

                    using (var stream = item.OpenReadStream())
                    {
                        await myBlob.UploadFromStreamAsync(stream);
                    }
                    // var uri = myBlob.Uri;
                    Upload myUpload = new Upload
                    {
                        Name = item.FileName,
                        Uri = myBlob.Uri.ToString()
                    };
                    TableOperation azInsert = TableOperation.InsertOrReplace(myUpload);
                    await _table.ExecuteAsync(azInsert);

                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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