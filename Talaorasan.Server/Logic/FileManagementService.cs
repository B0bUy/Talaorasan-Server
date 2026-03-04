using Talaorasan.Shared.Requests.Transaction;
using Talaorasan.Shared.Response;

namespace Talaorasan.Server.Logic
{
    public class FileManagementService
    {
        public async Task<TransactionResponseDto<string>> Upload(IFormFile file, long? size, string fileName = "")
        {
            try
            {
                if (file is null || file.Length == 0) return new TransactionResponseDto<string>() { Success = false, Message = "Uplaoding failed..." };
                //if (!file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase)) return new TransactionResponseDto<string>() { Success = false, Message = "Only images are allowed..." };
                Directory.CreateDirectory("Uploads");

                var safeName = Path.GetFileName(file.FileName);
                var ext = Path.GetExtension(safeName);
                var savedName = string.IsNullOrEmpty(fileName) ? $"{Guid.NewGuid():N}{ext}" : fileName;
                var path = Path.Combine("Uploads", savedName);

                await using var fs = File.Create(path);
                await file.CopyToAsync(fs);

                return new TransactionResponseDto<string>() { Success = true, Message = "File uploaded successfully.", Output = path };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new TransactionResponseDto<string>() { Success = false, Message = e.Message };
            }
        }
        public async Task<TransactionResponseDto<string>> Upload(FileDto file, string fileName = "")
        {
            try
            {
                if (file is null || file.Length == 0) return new TransactionResponseDto<string>() { Success = false, Message = "Uplaoding failed..." };
                //if (!file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase)) return new TransactionResponseDto<string>() { Success = false, Message = "Only images are allowed..." };
                Directory.CreateDirectory("Uploads");

                var safeName = Path.GetFileName(file.FileName);
                var ext = Path.GetExtension(safeName);
                var savedName = string.IsNullOrEmpty(fileName) ? $"{Guid.NewGuid():N}{ext}" : fileName;
                var path = Path.Combine("Uploads", savedName);

                await using var fs = File.Create(path);
                fs.Write(file.Content, 0, file.Length);
                fs.Close();

                return new TransactionResponseDto<string>() { Success = true, Message = "File uploaded successfully.", Output = path };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new TransactionResponseDto<string>() { Success = false, Message = e.Message };
            }
        }
    }
}
