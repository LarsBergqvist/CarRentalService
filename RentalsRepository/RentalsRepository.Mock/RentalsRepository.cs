using System;
using System.Collections.Generic;
using System.Linq;
using RentalsRepository.Contract;

namespace RentalsRepository.Mock
{
    public class RentalsRepository : IRentalsRepository
    {
        private ICollection<RentalInfo> _rentalItems;
        private int _nextRentalNumber = 1;

        public RentalsRepository()
        {
            _rentalItems = new List<RentalInfo>();
        }

        public RentalInfo AddNewRental(string vehicleTypeName, string regNo, DateTime rentalDate, double currentMileageKm, CustomerInfo customerInfo)
        {
            var rentalInfo = new RentalInfo()
            {
                CustomerInfo = customerInfo,
                RentalNumber = _nextRentalNumber.ToString(),
                Status = RentalInfo.ERentStatus.Rented,
                OriginalMileageKm = currentMileageKm,
                RentalDate = rentalDate,
                VehicleTypeName = vehicleTypeName,
                RegNo = regNo
            };

            _rentalItems.Add(rentalInfo);

            _nextRentalNumber++;

            return rentalInfo;
        }

        public RentalInfo UpdateRentalAsReturned(string rentalNumber, DateTime returnDate, double newMileageKm)
        {
            var rentalInfo = _rentalItems.ToList().Find(x => x.RentalNumber == rentalNumber);
            if (rentalInfo == null)
            {
                return null;
            }
            rentalInfo.ReturnDate = returnDate;
            rentalInfo.NewMileageKm = newMileageKm;
            rentalInfo.Status = RentalInfo.ERentStatus.Returned;

            return rentalInfo;
        }

        public RentalInfo GetRental(string rentalNumber)
        {
            var rentalInfo = _rentalItems.ToList().Find(x => x.RentalNumber == rentalNumber);
            return rentalInfo;
        }

        public bool VehicleIsOutForRent(string regNo)
        {
            var rentalInfo =
                _rentalItems.ToList().Find(x => x.RegNo == regNo && x.Status == RentalInfo.ERentStatus.Rented);
            if (rentalInfo != null)
            {
                return true;                
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<RentalInfo> GetAllRentals()
        {
            return _rentalItems;
        }

        public void RemoveAllRentals()
        {
            _rentalItems.Clear();
        }
    }

}
