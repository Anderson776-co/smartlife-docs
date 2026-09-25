using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FirebaseAuthPoC.Controller
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class PerfilController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            var uid = User.FindFirst("user_id")?.Value
                      ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            return Ok(new { uid, email, mensaje = "Token válido, acceso concedido" });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var uid = User.FindFirst("user_id")?.Value
                      ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(uid))
                return Unauthorized(new { mensaje = "No se pudo identificar al usuario" });

            try
            {
                await FirebaseAuth.DefaultInstance.RevokeRefreshTokensAsync(uid);
                return Ok(new { mensaje = "Sesión revocada correctamente" });
            }
            catch (FirebaseAuthException ex)
            {
                return StatusCode(500, new { mensaje = "Error al revocar sesión", detalle = ex.Message });
            }
        }
    }
}
