using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Manero.Lib.Context;

public class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : IdentityDbContext<IdentityUser>(options)
{

}
