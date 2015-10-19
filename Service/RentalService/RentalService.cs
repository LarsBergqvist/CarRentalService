using System;
using System.Collections.Generic;
using RentalsRepository.Contract;
using RentalService.Contract;
using VehicleTypes.Contract;

namespace RentalService
{
    public class RentalService : IRentalService, IVehicleTypesRepository
    {
        private readonly IRentalsRepository _rentalsRepository;
        private readonly VehicleTypes.Contract.Factory _vehicleTypesFactory;


        public RentalService(VehicleTypes.Contract.Factory vehicleTypesFactory, IRentalsRepository rentalsRepository)
        {
            _vehicleTypesFactory = vehicleTypesFactory;
            _rentalsRepository = rentalsRepository;
        }

        public IEnumerable<IVehicleType> GetAvailableVehicleTypes()
        {
            return _vehicleTypesFactory.GetAllVehicleTypes();
        }

        public RentalReceipt RentVehicle(RentalRequest rentalRequest)
        {
            var vehicleType = _vehicleTypesFactory.GetVehicleType(rentalRequest.VehicleTypeName);

            var receiptFromArgCheck = RentItem_CheckArguments(rentalRequest, vehicleType);
            if (receiptFromArgCheck.Status == ERentalRequestStatus.NotOk)
            {
                return receiptFromArgCheck;
            }

            var rentalInfo = _rentalsRepository.AddNewRental(vehicleType.Name, rentalRequest.RegNo, rentalRequest.RentDateTime, rentalRequest.CurrentMilageKm, rentalRequest.CustomerInfo);
            return new RentalReceipt() {RentalNumber = rentalInfo.RentalNumber, Status = ERentalRequestStatus.Ok, Message = string.Format("A rental for {0} has been registred.", vehicleType)};
        }

        public ReturnReceipt ReturnVehicle(ReturnRequest returnRequest)
        {
            var rentalInfo = _rentalsRepository.GetRental(returnRequest.RentalNumber);
            var receiptFromArgCheck = ReturnItem_CheckArguments(returnRequest, rentalInfo);
            if (receiptFromArgCheck.Status == ERentalReturnStatus.NotOk)
            {
                return receiptFromArgCheck;
            }

            rentalInfo = _rentalsRepository.UpdateRentalAsReturned(returnRequest.RentalNumber, returnRequest.ReturnDateTime,
                returnRequest.CurrentMilageKm);

            return new ReturnReceipt() { Status = ERentalReturnStatus.Ok, Message = string.Format("Rental with number {0} returned OK.",returnRequest.RentalNumber)};
        }

        public double GetPriceForRental(string rentalNumber, double dailyBaseCost, double milageKmBaseCost )
        {
            var rentalInfo = _rentalsRepository.GetRental(rentalNumber);
            if (rentalInfo.Status != RentalInfo.ERentStatus.Returned)
            {
                // Can not calculate price when there is no return date
                return 0.0;
            }

            var vehicleType = _vehicleTypesFactory.GetVehicleType(rentalInfo.VehicleTypeName);

            DateTime returnDate = rentalInfo.ReturnDate.GetValueOrDefault();
            TimeSpan rentalTime = (returnDate - rentalInfo.RentalDate);
            int numberOfRentalDays = RoundUpTimeSpanToClosestWholeDay(rentalTime);
            double numberOfMilageKm = rentalInfo.NewMileageKm.GetValueOrDefault() - rentalInfo.OriginalMileageKm;

            var costParameters = new CostParameters()
            {
                DailyBaseCost = dailyBaseCost,
                MilageKmBaseCost = milageKmBaseCost,
                NumberOfMilageKm = numberOfMilageKm,
                NumberOfRentalDays = numberOfRentalDays
            };

            double rentalToPay = vehicleType.GetRentalCost(costParameters);

            return rentalToPay;
        }

        public IEnumerable<RentalInfo> GetAllRentals()
        {
            return _rentalsRepository.GetAllRentals();
        }

        public void RemoveAllRentals()
        {
            _rentalsRepository.RemoveAllRentals();
        }

        public RentalInfo GetRental(string rentalNumber)
        {
            return _rentalsRepository.GetRental(rentalNumber);
        }

        private int RoundUpTimeSpanToClosestWholeDay(TimeSpan rentalTime)
        {
            int numberOfRentalDays = 0;
            if (rentalTime.Hours > 0 || rentalTime.Minutes > 0 || rentalTime.Seconds > 0)
            {
                numberOfRentalDays = rentalTime.Days + 1;
            }
            else
            {
                numberOfRentalDays = rentalTime.Days;
            }

            return numberOfRentalDays;
        }

        private RentalReceipt RentItem_CheckArguments(RentalRequest rentalRequest, IVehicleType vehicleType)
        {
            if (vehicleType == null)
            {
                return new RentalReceipt()
                {
                    Status = ERentalRequestStatus.NotOk,
                    Message = string.Format("The vehicle type {0} does not exist.", rentalRequest.VehicleTypeName)
                };
            }

            if (string.IsNullOrEmpty(rentalRequest.RegNo))
            {
                return new RentalReceipt()
                {
                    Status = ERentalRequestStatus.NotOk,
                    Message = "RegNo can not be empty."
                };
            }

            if (string.IsNullOrEmpty(rentalRequest.CustomerInfo.PersonNummer))
            {
                return new RentalReceipt()
                {
                    Status = ERentalRequestStatus.NotOk,
                    Message = "PersonNummer can not be empty."
                };
            }

            if (rentalRequest.CurrentMilageKm < 0)
            {
                return new RentalReceipt()
                {
                    Status = ERentalRequestStatus.NotOk,
                    Message = "Milage can not be < 0 km."
                };
            }

            if (_rentalsRepository.VehicleIsOutForRent(rentalRequest.RegNo))
            {
                return new RentalReceipt()
                {
                    Status = ERentalRequestStatus.NotOk,
                    Message = string.Format("Vehicle with RegNo {0} can not be rented as it is already out for rent.", rentalRequest.RegNo)
                };
            }

            return new RentalReceipt() { Status = ERentalRequestStatus.Ok };
        }

        private ReturnReceipt ReturnItem_CheckArguments(ReturnRequest returnRequest, RentalInfo rentalInfo)
        {
            if (rentalInfo == null)
            {
                return new ReturnReceipt() { Status = ERentalReturnStatus.NotOk, Message = string.Format("Rental with number {0} does not exist in the system.", returnRequest.RentalNumber) };
            }

            if (rentalInfo.Status == RentalInfo.ERentStatus.Returned)
            {
                return new ReturnReceipt()
                {
                    Status = ERentalReturnStatus.NotOk,
                    Message = "The vehicle is already registred as returned."
                };                
            }

            if (returnRequest.ReturnDateTime < rentalInfo.RentalDate)
            {
                return new ReturnReceipt()
                {
                    Status = ERentalReturnStatus.NotOk,
                    Message = "Return date can not be before rental date."
                };
            }

            if (returnRequest.CurrentMilageKm < rentalInfo.OriginalMileageKm)
            {
                return new ReturnReceipt()
                {
                    Status = ERentalReturnStatus.NotOk,
                    Message = "Milage on returned vehicle can not be less than original milage."
                };
            }

            return new ReturnReceipt() { Status = ERentalReturnStatus.Ok };
        }

    }
}
