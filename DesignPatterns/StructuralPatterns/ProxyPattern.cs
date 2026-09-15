using System;

namespace SoftwareDesignPrinciples.DesignPatterns.StructuralPatterns
{
    /// <summary>
    /// Proxy Pattern
    /// Provides a surrogate or placeholder for another object to control access to it.
    /// Common types: Virtual Proxy (lazy loading), Protection Proxy (access control).
    /// </summary>
    public static class ProxyPatternDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- Proxy Pattern Demo ---");
            Console.WriteLine("Scenario 1: Virtual Proxy for Lazy Loading");
            
            IImage image = new ProxyImage("high_res_photo.jpg");
            Console.WriteLine("Image object created. Actual image is NOT loaded yet.");
            
            // Image will be loaded from disk now
            image.Display();
            // Image will not be loaded again
            image.Display();

            Console.WriteLine("\nScenario 2: Protection Proxy for Access Control");
            IImage protectedImageUser = new ProtectedImage("secret_data.png", "User");
            protectedImageUser.Display();

            IImage protectedImageAdmin = new ProtectedImage("secret_data.png", "Admin");
            protectedImageAdmin.Display();
            
            Console.WriteLine();
        }
    }

    // Subject
    public interface IImage
    {
        void Display();
    }

    // Real Subject
    public class RealImage : IImage
    {
        private string _filename;

        public RealImage(string filename)
        {
            _filename = filename;
            LoadFromDisk();
        }

        private void LoadFromDisk()
        {
            Console.WriteLine($"Loading {_filename} from disk... (This is expensive)");
        }

        public void Display()
        {
            Console.WriteLine($"Displaying {_filename}");
        }
    }

    // Virtual Proxy
    public class ProxyImage : IImage
    {
        private RealImage? _realImage;
        private string _filename;

        public ProxyImage(string filename)
        {
            _filename = filename;
        }

        public void Display()
        {
            if (_realImage == null)
            {
                _realImage = new RealImage(_filename);
            }
            _realImage.Display();
        }
    }

    // Protection Proxy
    public class ProtectedImage : IImage
    {
        private ProxyImage _proxyImage;
        private string _userRole;

        public ProtectedImage(string filename, string userRole)
        {
            _proxyImage = new ProxyImage(filename);
            _userRole = userRole;
        }

        public void Display()
        {
            if (_userRole == "Admin")
            {
                Console.WriteLine("Access granted.");
                _proxyImage.Display();
            }
            else
            {
                Console.WriteLine("Access denied: Only Admins can view this image.");
            }
        }
    }
}
