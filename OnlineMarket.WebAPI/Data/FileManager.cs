namespace OnlineMarket.WebAPI.Data;

public class FileManager
{
    private readonly string _adPhotoPath;
    private readonly string _defaultAdPhotoName;
    private readonly string _defaultUserPhotoName;
    private readonly string _userPhotoPath;

    public FileManager(IConfiguration configuration)
    {
        _adPhotoPath = configuration["Path:AdPhoto"]!;
        _userPhotoPath = configuration["Path:UserPhoto"]!;
        _defaultUserPhotoName = configuration["Path:DefaultUserPhoto"]!;
        _defaultAdPhotoName = configuration["Path:DefaultAdPhoto"]!;

        if (!Directory.Exists(_adPhotoPath)) Directory.CreateDirectory(_adPhotoPath);
        if (!Directory.Exists(_userPhotoPath)) Directory.CreateDirectory(_userPhotoPath);
    }

    private static async Task<string> SaveFile(IFormFile? file, string basePath)
    {
        if (file is null) return basePath["wwwroot/".Length..];

        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

        await using var fileStream = new FileStream(Path.Combine(basePath, fileName), FileMode.Create);
        await file.CopyToAsync(fileStream);

        return basePath["wwwroot/".Length..] + fileName;
    }

    public async Task<string> SaveAdPhoto(IFormFile? file)
    {
        if (file is null) return await SaveFile(null, _defaultAdPhotoName);
        return await SaveFile(file, _adPhotoPath);
    }

    public async Task<string> SaveUserPhoto(IFormFile? file)
    {
        if (file is null) return await SaveFile(null, _defaultUserPhotoName);
        return await SaveFile(file, _userPhotoPath);
    }

    public bool DeleteAvatar(string avatarFileName)
    {
        // TODO:
        /*var path = Path.Combine(_basePath, avatarFileName);
        if (!File.Exists(path)) return false;

        try
        {
            File.Delete(path);
        }
        catch (Exception)
        {
            return false;
        }

        return true;*/

        throw new NotImplementedException();
    }

    public Task<string?> UpdateAvatar(string? oldAvatarFileName, string newFilePath)
    {
        // TODO:
        /*if (oldAvatarFileName is null) return await AddAvatar(newFilePath);

        var oldAvatarFilePath = Path.Combine(_basePath, oldAvatarFileName);

        try
        {
            await using var oldAvatarFileStream = new FileStream(oldAvatarFilePath, FileMode.Truncate);
            await using var fromFileStream = File.OpenRead(newFilePath);
            await fromFileStream.CopyToAsync(oldAvatarFileStream);
        }
        catch (Exception)
        {
            return null;
        }

        return oldAvatarFileName;*/

        throw new NotImplementedException();
    }
}