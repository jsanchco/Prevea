namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using IService.IService;
    using Model.Model;
    using System;

    #endregion

    public partial class Service
    {
        public List<DoctorMedicalExaminationEmployee> GetDoctorsMedicalExaminationEmployees()
        {
            return Repository.GetDoctorsMedicalExaminationEmployees();
        }

        public DoctorMedicalExaminationEmployee GetDoctorMedicalExaminationEmployeeById(int id)
        {
            return Repository.GetDoctorMedicalExaminationEmployeeById(id);
        }

        public DoctorMedicalExaminationEmployee GetDoctorMedicalExaminationEmployeeByDoctorId(int medicalExaminationEmployeeId,
            int doctorId)
        {
            return Repository.GetDoctorMedicalExaminationEmployeeByDoctorId(medicalExaminationEmployeeId, doctorId);
        }

        public Result SaveDoctorMedicalExaminationEmployee(DoctorMedicalExaminationEmployee doctorMedicalExaminationEmployee)
        {
            try
            {
                doctorMedicalExaminationEmployee = Repository.SaveDoctorMedicalExaminationEmployee(doctorMedicalExaminationEmployee);

                if (doctorMedicalExaminationEmployee == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación del DoctorMedicalExaminationEmployee",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación del DoctorMedicalExaminationEmployee se ha producido con éxito",
                    Object = doctorMedicalExaminationEmployee,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación del DoctorMedicalExaminationEmployee",
                    Object = doctorMedicalExaminationEmployee,
                    Status = Status.Error
                };
            }
        }

        public Result DeleteDoctorMedicalExaminationEmployee(int id)
        {
            try
            {
                var result = Repository.DeleteDoctorMedicalExaminationEmployee(id);

                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar DoctorMedicalExaminationEmployee",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "El Borrado de DoctorMedicalExaminationEmployee se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al Borrar DoctorMedicalExaminationEmployee",
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        public List<DateTime?> GetDatesByWorkSheet(int doctorId)
        {
            return Repository.GetDatesByWorkSheet(doctorId);
        }

        public int GetCountMedicalExaminationByState(int doctorId, DateTime date, EnMedicalExaminationState medicalExaminationState)
        {
            return Repository.GetCountMedicalExaminationByState(doctorId, date, medicalExaminationState);
        }
    }
}
