using CodeChallenge.Models;
using System;

namespace CodeChallenge.Services
{
    public interface ICompensationService
    {
        Compensation GetCompensationById(String id);
        Compensation CreateCompensation(Compensation compensation);
    }
}
