using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorEmployeeEFCore.Data
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly SqlDbContext _dbContext;

        public EmployeesController(SqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<List<Employee>> Get()
        {
            return await _dbContext.Employees.ToListAsync();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<bool> Create([FromBody]Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.Id = Guid.NewGuid().ToString();
                _dbContext.Add(employee);
                try
                {
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateException)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        [HttpGet]
        [Route("Details/{id}")]
        public async Task<Employee> Details(string id)
        {
            return await _dbContext.Employees.FindAsync(id);
        }

        [HttpPut]
        [Route("Edit/{id}")]
        public async Task<bool> Edit(string id, [FromBody]Employee employee)
        {
            if (id != employee.Id)
            {
                return false;
            }

            _dbContext.Entry(employee).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<bool> DeleteConfirmed(string id)
        {
            var patient = await _dbContext.Employees.FindAsync(id);
            if (patient == null)
            {
                return false;
            }

            _dbContext.Employees.Remove(patient);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
