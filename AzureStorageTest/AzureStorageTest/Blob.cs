namespace AzureStorageTest
{
	using Microsoft.WindowsAzure.Storage;
	using Microsoft.WindowsAzure.Storage.Blob;
	using System;
	using System.IO;
	using System.Threading.Tasks;

	public class Blob
	{
		private CloudStorageAccount storageAccount;
		private CloudBlobClient blobClient;
		private CloudBlobContainer blobContainer;
		private CloudBlockBlob blob;

		public Blob(string connectionString, string containerName, string blobName)
		{
			// Retrieve storage account from connection string.
			this.storageAccount = CloudStorageAccount.Parse(connectionString);

			// Create the blob client.
			this.blobClient = this.storageAccount.CreateCloudBlobClient();

			// Retrieve a reference to a container.
			this.blobContainer = this.blobClient.GetContainerReference(containerName);

			// Create the container if it doesn't already exist.
			this.blobContainer.CreateIfNotExists();

			// Retrieve reference to a blob
			this.blob = this.blobContainer.GetBlockBlobReference(blobName);
		}

		/// <summary>
		/// Get the ETag of blob
		/// Can be used to check content integrity
		/// </summary>
		public string GetETag()
		{
			return this.blob.Properties.ETag;
		}

		/// <summary>
		/// Get the last modified time of blob
		/// </summary>
		public DateTimeOffset? GetLastModifiedTime()
		{
			return this.blob.Properties.LastModified;
		}

		/// <summary>
		/// Delete the blob if it exists.
		/// </summary>
		public async Task DeleteAsync()
		{
			await this.blob.DeleteAsync();
		}

		/// <summary>
		/// Download the blob as text
		/// </summary>
		public async Task<string> DownloadTextAsync()
		{
			return await this.blob.DownloadTextAsync();
		}

		/// <summary>
		/// Download the blob as stream
		/// </summary>
		public async Task<Stream> DownloadStreamAsync()
		{
			using (var memoryStream = new MemoryStream())
			{
				await this.blob.DownloadToStreamAsync(memoryStream);
				return memoryStream;
			}
		}

		/// <summary>
		/// Upload the text to blob
		/// </summary>
		public async Task UploadTextAsync(string text)
		{
			await this.blob.UploadTextAsync(text);
		}

		/// <summary>
		/// Upload the stream to blob
		/// </summary>
		public async Task UploadStreamAsync(Stream stream)
		{
			await this.blob.UploadFromStreamAsync(stream);
		}

		/// <summary>
		/// Add new metadata or update the existing metadata of the blob for the given key
		/// </summary>
		public void AddOrUpdateMetadata(string key, string value)
		{
			this.blob.Metadata[key] = value;
		}
	}
}
