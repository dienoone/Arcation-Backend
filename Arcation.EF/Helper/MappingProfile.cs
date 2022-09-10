using Arcation.Core.Models.ArcationModels.Main;
using Arcation.Core.ViewModels.ArcationViewModel;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.EF.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Company Controller:
            CreateMap<Company, CompanyViewModel>().ReverseMap();
            CreateMap<Company, AddCompanyViewModel>().ReverseMap();
            CreateMap<Company, UpdateCompanyViewModel>().ReverseMap();

            // CompanyLocations Controller:
            CreateMap<Location, UpdateLocationViewModel>().ReverseMap();
            CreateMap<Location, AddLocationViewModel>().ReverseMap();
            CreateMap<Location, LocationViewModel>()
                .ForMember(dest => dest.Bands, src => src.MapFrom(src => src.BandLocations.Where(e => !e.IsDeleted).Select(b => new BandLocationDto
                {
                    BandLocationId = b.Id,
                    BandId = b.Band.Id,
                    BandName = b.Band.BandName

                })));

            // Band Contorller:
            CreateMap<Band, AddBandViewModel>().ReverseMap();
            CreateMap<Band, BandViewModel>().ReverseMap();

            // Bill Controller:
            CreateMap<Bill, BillDto>().ReverseMap();
            CreateMap<Bill, AddBillDto>().ReverseMap();
            CreateMap<Bill, UpdateBillDto>().ReverseMap();

            // Incomes Controller
            CreateMap<Income, IncomeDto>().ReverseMap();
            CreateMap<Income, AddIncomeDto>().ReverseMap();
            CreateMap<Income, UpdateIncomeDto>().ReverseMap();

            // BLWesteds:
            CreateMap<BLWested, BLWestedDto>().ReverseMap();
            CreateMap<BLWested, AddBLWestedDto>().ReverseMap();
            CreateMap<BLWested, UpdateBLWestedDto>().ReverseMap();

            // Leaders:
            CreateMap<Leader, LeaderInfoDto>().ReverseMap();
            CreateMap<Leader, LeadersPageDto>().ReverseMap();


            // Employess Controller:
            CreateMap<Employee, AddEmployeeDto>().ReverseMap();

            CreateMap<Employee, EmployeeDetailsDto>()
                .ForMember(dest => dest.TypeName, src => src.MapFrom(src => src.Type.Type))
                .ForMember(dest => dest.TypeId, src => src.MapFrom(src => src.Type.Id))
                .ForMember(dest => dest.EmployeeId, src => src.MapFrom(src => src.Id));

            CreateMap<Employee, EmployeePageDto>()
                .ForMember(dest => dest.TypeName, src => src.MapFrom(src => src.Type.Type))
                .ForMember(dest => dest.EmployeeId, src => src.MapFrom(src => src.Id));

            CreateMap<Employee, EmployeeBusinessDetailDto>()
                .ForMember(dest => dest.TotalSalary, src => src.MapFrom(src => src.BandLocationLeaderPeriodEmployees.Sum(n => n.BandLocationLeaderPeriodEmployeePeriods.Sum(e => e.BandLocationLeaderPeriodEmployeePeriodAttendances.Sum(e => e.WorkingHours) * e.EmployeeSalary))))
                .ForMember(dest => dest.TotalDays, src => src.MapFrom(src => src.BandLocationLeaderPeriodEmployees.Sum(n => n.BandLocationLeaderPeriodEmployeePeriods.Sum(e => e.BandLocationLeaderPeriodEmployeePeriodAttendances.Sum(e => e.WorkingHours)))))
                .ForMember(dest => dest.TotalPayied, src => src.MapFrom(src => src.BandLocationLeaderPeriodEmployees.Sum(n => n.BandLocationLeaderPeriodEmployeePeriods.Sum(e => e.PayiedValue + e.BandLocationLeaderPeriodEmployeePeriodAttendances.Sum(e => e.BorrowValue)))));

            // ToDo:
            CreateMap<Location, LocationNames>()
                .ForMember(dest => dest.LocationId, src => src.MapFrom(src => src.Id))
                .ForMember(dest => dest.LocationName, src => src.MapFrom(src => src.LocationName));


            CreateMap<Employee, AddEmployeeToPeriodRequireDto>()
                .ForMember(dest => dest.Type, src => src.MapFrom(src => src.Type.Type));

            //Leader Transiction Controller:
            CreateMap<Transiction, LeaderTransictionDto>().ReverseMap();
            CreateMap<Transiction, AddLeaderTransictionDto>().ReverseMap();
            CreateMap<Transiction, UpdateLeaderTransictionDto>().ReverseMap();

            //Leader Transiction Controller:
            CreateMap<Wested, LeaderWestedDto>().ReverseMap();
            CreateMap<Wested, AddLeaderWestedDto>().ReverseMap();
            CreateMap<Wested, UpdateLeaderWestedDto>().ReverseMap();

            // Employee Type Controller:
            CreateMap<EmployeeType, EmployeeTypeDto>().ReverseMap();
            CreateMap<EmployeeType, AddEmployeeTypeDto>().ReverseMap();
            CreateMap<EmployeeType, UpdateEmployeeTypeDto>().ReverseMap();

            // BandLocationLeaders Controller:
            CreateMap<BandLocationLeader, AddBandLeaderDto>().ReverseMap();
            CreateMap<BandLocationLeader, BandLocationLeaderPeriodsDto>()
                .ForMember(dest => dest.BandLocationLeaderId, src => src.MapFrom(e => e.Id))
                .ForMember(dest => dest.LeaderName, src => src.MapFrom(e => e.Leader.Name))
                .ForMember(dest => dest.LeaderSalary, src => src.MapFrom(e => e.Leader.Salary))
                .ForMember(dest => dest.LeaderPeriods, src => src.MapFrom(e => e.BandLocationLeaderPeriods.Select(n => new LeaderPeriods
                {
                    bandLocationLeaderPeriodId = n.Id,
                    PeriodName = n.Period.Name,
                    periodState = n.Period.State

                })));

            CreateMap<Period, AddPeriodRequireDto>().ReverseMap();

            // Transictions Controller:
            CreateMap<Transiction, AddTransictionDto>().ReverseMap();
            CreateMap<Transiction, UpdateTransictionDto>().ReverseMap();
            CreateMap<Transiction, TransictionDto>().ReverseMap();

            // EmployeePeriods Controller:
            CreateMap<BandLocationLeaderPeriodEmployee, BandLocationLeaderPeriodEmployeeDto>()
               .ForMember(dest => dest.BandLocationLeaderPeriodEmployeeId, src => src.MapFrom(src => src.Id))
               .ForMember(dest => dest.EmployeeId, src => src.MapFrom(src => src.Employee.Id))
               .ForMember(dest => dest.EmployeeType, src => src.MapFrom(src => src.Employee.Type.Type))
               .ForMember(dest => dest.Periods, src => src.MapFrom(c => c.BandLocationLeaderPeriodEmployeePeriods.Select(n => new PeriodsList
               {
                   BandLocationLeaderPeriodEmployeePeriodId = n.Id,
                   StartingDate = n.StartingDate,
                   EndingDate = n.EndingDate,
                   State = n.State

               })));

            // Leaders Controller:
            CreateMap<Leader, AddLeaderToBandRequireDto>().ReverseMap();

            CreateMap<Leader, EmployeeBusinessDetailDto>()
                .ForMember(dest => dest.TotalSalary, src => src.MapFrom(e => e.BandLocationLeaders.Sum(e => e.BandLocationLeaderPeriods.Sum(e => e.LeaderSalary * e.Attendances.Sum(e => e.WorkingHours)))))
                .ForMember(dest => dest.TotalPayied, src => src.MapFrom(e => e.BandLocationLeaders.Sum(e => e.BandLocationLeaderPeriods.Sum(e => e.TotalPaied) + e.BandLocationLeaderPeriods.Sum(e => e.Attendances.Sum(e => e.BorrowValue)))))
                .ForMember(dest => dest.TotalDays, src => src.MapFrom(e => e.BandLocationLeaders.Sum(e => e.BandLocationLeaderPeriods.Sum(e => e.Attendances.Where(e => e.AttendanceState).Sum(e => e.WorkingHours)))));

            CreateMap<BandLocationLeader, LeaderLocationDetail>()
                .ForMember(dest => dest.LocationName, src => src.MapFrom(e => e.BandLocation.Location.LocationName))
                .ForMember(dest => dest.BandName, src => src.MapFrom(e => e.BandLocation.Band.BandName))
                .ForMember(dest => dest.LeaderSalary, src => src.MapFrom(e => e.Leader.Salary))
                .ForMember(dest => dest.TotalDays, src => src.MapFrom(e => e.BandLocationLeaderPeriods.Sum(e => e.Attendances.Sum(e => e.WorkingHours))))
                .ForMember(dest => dest.TotalSalary, src => src.MapFrom(e => e.BandLocationLeaderPeriods.Sum(e => e.LeaderSalary * e.Attendances.Sum(e => e.WorkingHours))))
                .ForMember(dest => dest.TotalBorrow, src => src.MapFrom(e => e.BandLocationLeaderPeriods.Sum(e => e.Attendances.Sum(e => e.BorrowValue))))
                .ForMember(dest => dest.TotalPaied, src => src.MapFrom(e => e.BandLocationLeaderPeriods.Sum(e => e.TotalPaied)));

            CreateMap<BandLocationLeader, GlobalLeaderDetail>()
                .ForMember(dest => dest.LeaderPeriods, src => src.MapFrom(src => src.BandLocationLeaderPeriods.Select(n => new LeaderPeriodDtos
                {
                    BandLocationLeaderPeriodId = n.Id,
                    PeriodName = n.Period.Name,
                    PeriodState = n.Period.State
                })));

            CreateMap<BandLocationLeaderPeriod, LeaderPeriodDetail>()
                .ForMember(dest => dest.BandName, src => src.MapFrom(e => e.BandLocationLeader.BandLocation.Band.BandName))
                .ForMember(dest => dest.LocationName, src => src.MapFrom(e => e.BandLocationLeader.BandLocation.Location.LocationName))
                .ForMember(dest => dest.LeaderName, src => src.MapFrom(e => e.BandLocationLeader.Leader.Name))
                .ForMember(dest => dest.LeaderSalary, src => src.MapFrom(e => e.LeaderSalary))
                .ForMember(dest => dest.TotalDays, src => src.MapFrom(e => e.Attendances.Sum(e => e.WorkingHours)))
                .ForMember(dest => dest.TotalSalary, src => src.MapFrom(e => e.LeaderSalary * e.Attendances.Sum(e => e.WorkingHours)))
                .ForMember(dest => dest.TotalPaied, src => src.MapFrom(e => e.TotalPaied))
                .ForMember(dest => dest.TotalBorrow, src => src.MapFrom(e => e.Attendances.Sum(e => e.BorrowValue)));

            // LeaderPeriod Controller:
            CreateMap<BandLocationLeaderPeriod, PeriodDetail>()
                .ForMember(dest => dest.TotalTransictions, src => src.MapFrom(src => src.Transactions.Where(e => !e.IsDeleted).Sum(e => e.Value)))
                .ForMember(dest => dest.TotalWesteds, src => src.MapFrom(src => src.Westeds.Where(e => !e.IsDeleted).Sum(e => e.Value)))
                .ForMember(dest => dest.TotalDays, src => src.MapFrom(src => src.Attendances.Where(e => !e.IsDeleted).Sum(e => e.WorkingHours)))
                .ForMember(dest => dest.TotalBorrows, src => src.MapFrom(src => src.Attendances.Where(e => !e.IsDeleted).Sum(e => e.BorrowValue)
                + src.Attendances.Where(e => !e.IsDeleted).Sum(e => e.BandLocationLeaderPeriodEmployeePeriodAttendances.Where(e => !e.IsDeleted).Sum(e => e.BorrowValue))));

            CreateMap<BandLocationLeaderPeriod, LeaderDetail>()
                .ForMember(dest => dest.LeaderName, src => src.MapFrom(src => src.BandLocationLeader.Leader.Name))
                .ForMember(dest => dest.LeaderSalary, src => src.MapFrom(src => src.LeaderSalary))
                .ForMember(dest => dest.IsEnded, src => src.MapFrom(src => src.PayiedState))
                .ForMember(dest => dest.TotalPaied, src => src.MapFrom(src => src.TotalPaied))
                .ForMember(dest => dest.TotalBorrow, src => src.MapFrom(src => src.Attendances.Where(e => !e.IsDeleted).Sum(e => e.BorrowValue)))
                .ForMember(dest => dest.TotalDays, src => src.MapFrom(src => src.Attendances.Where(e => !e.IsDeleted && e.AttendanceState).Sum(e => e.WorkingHours)));

            CreateMap<BandLocationLeaderPeriod, EmployeesDetail>()
                .ForMember(dest => dest.TotalSalaryOfEmployess, src => src.MapFrom(src => src.BandLocationLeaderPeriodEmployees.Where(e => !e.IsDeleted)
                .Sum(e => e.EmployeeSalay * e.BandLocationLeaderPeriodEmployeePeriods.Where(e => !e.IsDeleted).Sum(e => e.BandLocationLeaderPeriodEmployeePeriodAttendances.Where(e => !e.IsDeleted && e.State).Sum(e => e.WorkingHours)))))
                .ForMember(dest => dest.TotalPaied, src => src.MapFrom(src => src.BandLocationLeaderPeriodEmployees.Where(e => !e.IsDeleted).Sum(e => e.BandLocationLeaderPeriodEmployeePeriods
                .Where(e => !e.IsDeleted).Sum(e => e.PayiedValue))));

            // EmployeeMainPeriodsPageDto
            CreateMap<BandLocationLeaderPeriodEmployee, EmployeeMainPeriodDetailPageDto>()
                .ForMember(dest => dest.BandLocationLeaderPeriodEmployeeId, src => src.MapFrom(src => src.Id))
                .ForMember(dest => dest.LocationName, src => src.MapFrom(src => src.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Location.LocationName))
                .ForMember(dest => dest.TotalDays, src => src.MapFrom(src => src.BandLocationLeaderPeriodEmployeePeriods.Sum(e => e.BandLocationLeaderPeriodEmployeePeriodAttendances.Sum(e => e.WorkingHours))))
                .ForMember(dest => dest.TotalSalary, src => src.MapFrom(src => src.BandLocationLeaderPeriodEmployeePeriods.Sum(e => e.EmployeeSalary)))
                .ForMember(dest => dest.TotalBorrow, src => src.MapFrom(src => src.BandLocationLeaderPeriodEmployeePeriods.Sum(e => e.BandLocationLeaderPeriodEmployeePeriodAttendances.Sum(e => e.BorrowValue))))
                .ForMember(dest => dest.TotalPaied, src => src.MapFrom(src => src.BandLocationLeaderPeriodEmployeePeriods.Sum(e => e.PayiedValue)))
                .ForMember(dest => dest.state, src => src.MapFrom(src => src.State))
                .ForMember(dest => dest.SubPeriods, src => src.MapFrom(src => src.BandLocationLeaderPeriodEmployeePeriods.Select(n => new EmployeeChildPeriodDetailPageDto
                {
                    BandLocationLeaderPeriodEmployeePeriodId = n.Id,
                    From = n.StartingDate,
                    To = n.EndingDate,
                    State = n.State

                })));

            CreateMap<BandLocationLeaderPeriodEmployeePeriod, SubPeriodDetailDto>()
               .ForMember(dest => dest.LeaderName, src => src.MapFrom(e => e.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod.BandLocationLeader.Leader.Name))
               .ForMember(dest => dest.BandName, src => src.MapFrom(e => e.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod.BandLocationLeader.BandLocation.Band.BandName))
               .ForMember(dest => dest.MainPeriodId, src => src.MapFrom(e => e.BandLocationLeaderPeriodEmployee.BandLocationLeaderPeriod.Id))
               .ForMember(dest => dest.BandLocationLeaderPeriodEmployeePeriodId, src => src.MapFrom(e => e.Id))
               .ForMember(dest => dest.EmployeeType, src => src.MapFrom(e => e.BandLocationLeaderPeriodEmployee.Employee.Type.Type))
               .ForMember(dest => dest.EmployeeSalary, src => src.MapFrom(e => e.EmployeeSalary))
               .ForMember(dest => dest.TotalDays, src => src.MapFrom(e => e.BandLocationLeaderPeriodEmployeePeriodAttendances.Sum(e => e.WorkingHours)))
               .ForMember(dest => dest.TotalSalary, src => src.MapFrom(e => e.BandLocationLeaderPeriodEmployeePeriodAttendances.Sum(e => e.WorkingHours) * e.EmployeeSalary))
               .ForMember(dest => dest.TotalPaied, src => src.MapFrom(e => e.PayiedValue))
               .ForMember(dest => dest.TotalBorrow, src => src.MapFrom(e => e.BandLocationLeaderPeriodEmployeePeriodAttendances.Sum(e => e.BorrowValue)))
               .ForMember(dest => dest.state, src => src.MapFrom(e => e.State));

            // Periods Controller:
            CreateMap<Period, AddPeriodDto>().ReverseMap();

            CreateMap<Period, AllPeriodDto>()
                .ForMember(dest => dest.PeriodId, src => src.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, src => src.MapFrom(src => src.Name))
                .ForMember(dest => dest.State, src => src.MapFrom(src => src.State));

            CreateMap<Period, PeriodDetailDto>()
                .ForMember(dest => dest.PeriodId, src => src.MapFrom(src => src.Id))
                .ForMember(dest => dest.PeriodName, src => src.MapFrom(src => src.Name))
                .ForMember(dest => dest.StartingDate, src => src.MapFrom(src => src.StartingDate))
                .ForMember(dest => dest.EndingDate, src => src.MapFrom(src => src.EndingDate))
                .ForMember(dest => dest.PeriodState, src => src.MapFrom(src => src.State))
                .ForMember(dest => dest.LocationName, src => src.MapFrom(src => src.BandLocation.Location.LocationName))
                .ForMember(dest => dest.BandName, src => src.MapFrom(src => src.BandLocation.Band.BandName))
                .ForMember(dest => dest.TotalTransictions, src => src.MapFrom(src => src.BandLocationLeaderPeriods.Sum(e => e.Transactions.Sum(e => e.Value))))
                .ForMember(dest => dest.TotalWesteds, src => src.MapFrom(src => src.BandLocationLeaderPeriods.Sum(e => e.Westeds.Sum(e => e.Value))))
                .ForMember(dest => dest.TotalDays, src => src.MapFrom(src => src.BandLocationLeaderPeriods.Sum(e => e.Attendances.Sum(e => e.WorkingHours))))
                .ForMember(dest => dest.CountOfLeaders, src => src.MapFrom(src => src.BandLocationLeaderPeriods.Count))
                .ForMember(dest => dest.CountOfEmployees, src => src.MapFrom(src => src.BandLocationLeaderPeriods.Sum(e => e.BandLocationLeaderPeriodEmployees.Count)))
                .ForMember(dest => dest.TotalSalaryOfEmployees, src => src.MapFrom(src => src.BandLocationLeaderPeriods.Sum(e => e.BandLocationLeaderPeriodEmployees.Sum(e => e.BandLocationLeaderPeriodEmployeePeriods.Sum(e => e.EmployeeSalary * e.BandLocationLeaderPeriodEmployeePeriodAttendances.Sum(e => e.WorkingHours))) + e.LeaderSalary * e.Attendances.Sum(e => e.WorkingHours))))
                .ForMember(dest => dest.TotalPaied, src => src.MapFrom(src => src.BandLocationLeaderPeriods.Sum(e => e.TotalPaied + e.BandLocationLeaderPeriodEmployees.Sum(e => e.BandLocationLeaderPeriodEmployeePeriods.Sum(e => e.PayiedValue)))))
                .ForMember(dest => dest.RemainderFromTransictions, src => src.MapFrom(src => src.BandLocationLeaderPeriods.Sum(e => e.Transactions.Sum(e => e.Value) - (e.Westeds.Sum(e => e.Value) + e.Attendances.Sum(e => e.BorrowValue) + e.BandLocationLeaderPeriodEmployees.Sum(e => e.BandLocationLeaderPeriodEmployeePeriods.Sum(e => e.BandLocationLeaderPeriodEmployeePeriodAttendances.Sum(e => e.BorrowValue)))))));

            CreateMap<Period, GlobalSinglePeriod>()
                .ForMember(dest => dest.PeriodLeaders, src => src.MapFrom(e => e.BandLocationLeaderPeriods.Select(n => new PeriodLeaders
                {
                    BandLocationLeaderPeriodId = n.Id,
                    LeaderID = n.BandLocationLeader.Leader.Id,
                    LeaderName = n.BandLocationLeader.Leader.Name,
                    LeaderState = n.State
                })));

            // Attendance Controller:
            CreateMap<Attendance, AttendanceDto>()
                .ForMember(dest => dest.AttendanceId, src => src.MapFrom(src => src.Id))
                .ForMember(dest => dest.BandLocationLeaderPeriodId, src => src.MapFrom(src => src.BandLocationLeaderPeriod.Id))
                .ForMember(dest => dest.LeaderName, src => src.MapFrom(src => src.BandLocationLeaderPeriod.BandLocationLeader.Leader.Name))
                .ForMember(dest => dest.AttendanceState, src => src.MapFrom(src => src.AttendanceState))
                .ForMember(dest => dest.BorrowValue, src => src.MapFrom(src => src.BorrowValue))
                .ForMember(dest => dest.WorkingHours, src => src.MapFrom(src => src.WorkingHours))
                .ForMember(dest => dest.Employees, src => src.MapFrom(src => src.BandLocationLeaderPeriodEmployeePeriodAttendances
                .Select(n => new AttendanceEmployeeDto
                {
                    BandLocationLeaderPeriodEmployeePeriodAttendaceId = n.Id,
                    EmployeeName = n.BandLocationLeaderPeriodEmployeePeriod.BandLocationLeaderPeriodEmployee.Employee.Name,
                    EmployeeType = n.BandLocationLeaderPeriodEmployeePeriod.BandLocationLeaderPeriodEmployee.Employee.Type.Type,
                    AttendanceState = n.State,
                    BorrowValue = n.BorrowValue,
                    WorkingHours = n.WorkingHours

                })));

            CreateMap<Attendance, AllAttendances>()
                .ForMember(dest => dest.AttendanceId, src => src.MapFrom(src => src.Id))
                .ForMember(dest => dest.Date, src => src.MapFrom(src => src.Date))
                .ForMember(dest => dest.ended, src => src.MapFrom(src => src.ended));
        

            // Reports Controller:
            CreateMap<Company, CompanyReprots>()
                .ForMember(dest => dest.CompanyId, src => src.MapFrom(src => src.Id))
                .ForMember(dest => dest.CompanyName, src => src.MapFrom(src => src.Name));

            CreateMap<Location, CompanyLocationsReports>()
                .ForMember(dest => dest.LocationId, src => src.MapFrom(src => src.Id))
                .ForMember(dest => dest.LocationName, src => src.MapFrom(src => src.LocationName));

            CreateMap<BandLocation, CompanyLocationBandReports>()
                .ForMember(dest => dest.BandId, src => src.MapFrom(src => src.Id))
                .ForMember(dest => dest.BandName, src => src.MapFrom(src => src.Band.BandName));

            // Tool Controller : 
            CreateMap<Tool, ToolViewModel>()
                .ForMember(dest => dest.ToolName, src => src.MapFrom(src => src.ToolName))
                .ForMember(dest => dest.ToolCount, src => src.MapFrom(src => src.Count));

            // Extract Controller:
            CreateMap<Extract, AllExtracts>()
                .ForMember(dest => dest.ExtractId, src => src.MapFrom(src => src.ExtractId))
                .ForMember(dest => dest.ExtractName, src => src.MapFrom(src => src.ExtractName));

            // LeaderApp:
            CreateMap<BandLocationLeaderPeriod, PeriodsLeaderApp>()
                .ForMember(e => e.BandLocationLeaderPeriodId, src => src.MapFrom(e => e.Id))
                .ForMember(e => e.PeriodName, src => src.MapFrom(e => e.Period.Name))
                .ForMember(e => e.PeriodState, src => src.MapFrom(e => e.State))
                .ForMember(e => e.PeriodId, src => src.MapFrom(e => e.Period.Id));

        }
    }
}
