using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Distribuidora.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting; // <-- SOLUCIÓN 1: Añadir para IWebHostEnvironment
using System.IO;                   // <-- SOLUCIÓN 2: Añadir para Path, Directory, etc.

namespace Distribuidora.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment; // <-- NUEVO: Para saber dónde guardar archivos

        public IndexModel(
            UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager,
            IWebHostEnvironment webHostEnvironment) // <-- NUEVO
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment; // <-- NUEVO
        }

        public string? Username { get; set; }
        public string? AvatarUrl { get; set; } // <-- NUEVO: Para mostrar el avatar actual

        [TempData]
        public string? StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string? PhoneNumber { get; set; } // <-- Acepta nulos

            [Display(Name = "Nuevo Avatar")]
            public IFormFile? AvatarFile { get; set; } // <-- Para recibir el archivo. El AvatarUrl no es necesario aquí.
        }

        private async Task LoadAsync(Usuario user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var avatarUrl = user.AvatarUrl; // <-- NUEVO

            Username = userName;
            AvatarUrl = avatarUrl; // <-- NUEVO: Pasamos la URL a la vista

            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            // --- LÓGICA PARA SUBIR EL AVATAR (¡NUEVO!) ---
            if (Input.AvatarFile != null)
            {
                // 1. Dónde guardar: wwwroot/Uploads/Avatares
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string uploadsDir = Path.Combine(wwwRootPath, "Uploads", "Avatares");

                // 2. Si no existe la carpeta, la crea
                if (!Directory.Exists(uploadsDir))
                {
                    Directory.CreateDirectory(uploadsDir);
                }

                // 3. Nombre único para el archivo (para evitar sobreescribir)
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(Input.AvatarFile.FileName);
                string filePath = Path.Combine(uploadsDir, fileName);

                // 4. Guarda el archivo en el disco
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.AvatarFile.CopyToAsync(fileStream);
                }

                // 5. Guarda la RUTA en la BD
                user.AvatarUrl = "/Uploads/Avatares/" + fileName; 
            }
            // --- FIN LÓGICA DE SUBIDA ---

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _userManager.UpdateAsync(user); // <-- Guarda el AvatarUrl en la BD
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Tu perfil ha sido actualizado";
            return RedirectToPage();
        }
    }
}