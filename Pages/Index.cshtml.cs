using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace example.fileuploader.host.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private const string _directory = "Uploads";

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPostAsync(List<IFormFile> files)
    {
        long size = files.Sum(f => f.Length);
        var filePaths = new List<string>();
        foreach (var formFile in files)
        {
            if (formFile.Length > 0)
            {
                if (!Directory.Exists(_directory))
                    Directory.CreateDirectory(_directory);
                dynamic formFileDynamic = formFile;
                var filePath = Path.Combine(_directory, formFileDynamic.FileName);
                filePaths.Add(filePath);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }
            }
        }
        return new OkObjectResult(new { count = files.Count, size, filePaths });
    }
}
