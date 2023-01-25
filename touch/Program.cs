using System.Security;

foreach (var file in args)
{
    try
    {
        try
        {
            var path = Path.GetFullPath(file);
            try
            {
                if (Path.GetDirectoryName(path) is string dir)
                    EnsureDirectoryExist(new DirectoryInfo(dir));

                File.WriteAllBytes(path, Array.Empty<byte>());
            }
            catch (UnauthorizedAccessException)
            {
                Console.Error.WriteLine($"You do not have access to (part of) {path}");
            }
            catch (IOException)
            {
                Console.Error.WriteLine("An IO error occurred");
            }
        }
        catch (ArgumentException)
        {
            Console.Error.WriteLine($"{file} contains embedded null characters");
        }
    }
    catch (PathTooLongException)
    {
        Console.Error.WriteLine($"{Path.Combine(Directory.GetCurrentDirectory(), file)} is too long");
    }
    catch (SecurityException)
    {
        Console.Error.WriteLine("A security error occurred");
    }
}

static void EnsureDirectoryExist(DirectoryInfo? directory)
{
    if (directory is null || directory.Exists)
        return;

    EnsureDirectoryExist(directory.Parent);

    try
    {
        directory.Create();
    }
    catch (IOException)
    {
        Console.Error.WriteLine("Failed to create directory");
    }
}