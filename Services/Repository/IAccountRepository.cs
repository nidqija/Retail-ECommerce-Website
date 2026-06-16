namespace RetailECommerce.Services.Repository;
using RetailECommerce.Models;
using Microsoft.EntityFrameworkCore;


public interface IAccountRepository
{
    void ChangePassword( string newPassword , string email);
}