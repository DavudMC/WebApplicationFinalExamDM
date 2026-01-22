using System.Threading.Tasks;

namespace WebApplicationFinalExamDM.Helpers
{
    public static class ExtensionMethods
    {
        public static bool CheckType(this IFormFile file, string type)
        {
            return file.ContentType.Contains(type);
        }
        public static bool CheckSize(this IFormFile file, int mb)
        {
            return file.Length < mb * 1024 * 1024;
        }
        public static async Task<string> FileUploadAsync(this IFormFile file, string folderPath)
        {
            string uniqueImagePath = Guid.NewGuid().ToString() + file.FileName;
            string path = Path.Combine(folderPath, uniqueImagePath);
            using FileStream stream = new(path, FileMode.Create);
            await file.CopyToAsync(stream);
            return uniqueImagePath;
        }
        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
