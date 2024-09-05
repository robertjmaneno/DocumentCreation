using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebClient.Models;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;

namespace WebClient.Controllers
{
    public class FolderController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        private readonly ILogger<FolderController> _logger;

        public FolderController(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<FolderController> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"];
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Fetching root folders");
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/Folders");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("API Response Content: {Content}", content);

                var allFolders = DeserializeWithErrorHandling<List<FolderDto>>(content);

                var rootFolders = allFolders.Where(f => f.ParentId == null).ToList();

                var viewModel = new FolderContentsDto
                {
                    Path = "",
                    Subfolders = rootFolders,
                    Files = new List<FileDto>()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching root folders");
                return View("Error", new ErrorViewModel { Message = "An error occurred while fetching folders. Please try again later." });
            }
        }

        public async Task<IActionResult> ViewFolder(string path)
        {
            try
            {
                _logger.LogInformation("Attempting to view folder with path: {Path}", path);

                var requestUrl = $"{_apiBaseUrl}/api/Folders/contents?path={path}";
                _logger.LogInformation("Sending request to API: {Url}", requestUrl);

                var response = await _httpClient.GetAsync(requestUrl);
                _logger.LogInformation("API Response Status Code: {StatusCode}", response.StatusCode);

                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("API Response Content: {Content}", content);

                response.EnsureSuccessStatusCode();

                var result = DeserializeWithErrorHandling<ApiResponse<FolderContentsDto>>(content);

                if (result.Success)
                {
                    return View(result.Folder);
                }
                else
                {
                    _logger.LogWarning("API returned unsuccessful response: {Message}", result.Message);
                    return View("Error", new ErrorViewModel { Message = result.Message });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error viewing folder");
                return View("Error", new ErrorViewModel { Message = "An error occurred while viewing the folder. Please try again later." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFolder([FromBody] CreateFolderDto createFolderDto)
        {
            try
            {
                _logger.LogInformation("Attempting to create folder: {FolderName} with ParentId: {ParentId}", createFolderDto.Name, createFolderDto.ParentId);

                var response = await _httpClient.PostAsJsonAsync($"{_apiBaseUrl}/api/Folders", createFolderDto);

                _logger.LogInformation("API request sent. Status code: {StatusCode}", response.StatusCode);

                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("API Response Content: {Content}", content);

                response.EnsureSuccessStatusCode();

                var result = DeserializeWithErrorHandling<ApiResponse<FolderDto>>(content);
                return Json(new { success = true, folder = result.Folder });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating folder");
                return Json(new { success = false, message = "Unable to create folder. Please try again later." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadDocument(IFormFile file, string parentPath)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    _logger.LogWarning("Attempt to upload empty file to {ParentPath}", parentPath);
                    return Json(new { success = false, message = "File is empty" });
                }

                _logger.LogInformation("Uploading file {FileName} to {ParentPath}", file.FileName, parentPath);
                using var content = new MultipartFormDataContent();
                using var fileContent = new StreamContent(file.OpenReadStream());
                content.Add(fileContent, "file", file.FileName);
                content.Add(new StringContent(parentPath), "parentPath");

                var response = await _httpClient.PostAsync($"{_apiBaseUrl}/api/Documents", content);
                response.EnsureSuccessStatusCode();

                _logger.LogInformation("File {FileName} uploaded successfully", file.FileName);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading document {FileName} to {ParentPath}", file?.FileName, parentPath);
                return Json(new { success = false, message = "Unable to upload document. Please try again later." });
            }
        }

        private T DeserializeWithErrorHandling<T>(string json)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                Error = (sender, args) =>
                {
                    _logger.LogError("JSON Deserialization Error: {ErrorMessage}", args.ErrorContext.Error.Message);
                    args.ErrorContext.Handled = true;
                },
                MissingMemberHandling = MissingMemberHandling.Error
            };

            return JsonConvert.DeserializeObject<T>(json, settings);
        }
    }
}