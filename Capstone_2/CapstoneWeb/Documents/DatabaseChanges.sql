-- Author: Raj K
-- Date: 09-01-2018
-- Changes
-- 1. Altered table 't_employee' & 't_employeelog' tables. 
-- Added new column 'SubDesignation' after 'DesignationId' column.
-- 2. INSERT INTO m_doctorspeciality (NAME, IsActive, CreatedDate, CreatedUserId)
--		VALUES ('Supporting Staff', 1, SYSDATETIME(), 1)
-- 3. Added new SP 'SavePCPNDTEmployee'

-- Author : Chandu
-- Date : 09-01-2018
-- Changes :
-- 1.Altered SP : SP_SaveUserRegistration (apply UPPER() for Name & PANCard).

-- Author : Chandu
-- Date : 10-01-2018
-- Changes :
-- 1.Altered SP : SaveApplicantDetails (apply UPPER() for Name & PANCard).

-- Author : Siva Katta
-- Date : 11-01-2018
-- Changes :
-- 1.Altered SP : SaveApplicantDetails (Added ,@ApplicantPhotoPath VARCHAR(500) Parameter).
-- 2. Altered table 't_applicant'. 
--    Added new column ApplicantPhotoPath.

-- Author : Raj K
-- Date : 11-01-2018
-- Changes :
-- 1. Added new SP 'CheckforEmployeeRegistration'
-- 2. Altered SP 'GetPCPNDTData'
-- 3. Altered SP '[SaveEmployeeAmendment]'


-- Author : Mounika V
-- Date : 11-01-2018
-- Changes :
-- 1. Added 2new columns in T_Equipment (NOCFilePath,TransferCertificateFilePath)
-- 2. Altered InsertEquipmentBulk
-- 3. Modifiled Table type EquipmentModel
-- Date :12-01-2018
-- Changes:
-- 1. Added 2new colums in T_Equipment (InstallationCertificatePath,ImagePath)
-- 2. Altered InsertEquipmentBulk
-- 3. Modifiled Table type EquipmentModel

-- Author : Chandu
-- Date : 12-01-2018
-- Changes : 
-- 1. Altered table "t_applicant"
--  Added new 2 columns AadharCardPath and PANCardPath
-- 2. Altered SP 'SaveApplicantDetails'
-- 3.Altered SP 'SaveFacilityDetails' ( apply UPPER() to facilityName)

-- Author : Siva Katta
-- Date : 16-01-2018
-- Changes :
-- 1.Altered SP : GetPCPNDTData (Added column ApplicantPhotoPath).

-- Author : Chandu
-- Date : 16-01-2018
-- Changes :
-- 1.Altered SP : GetPCPNDTData (Added columns AadharCardPath & PANCardPath).

-- Author : Mounika
-- Date : 16-01-2018
-- Changes :
-- 1. Added column Invoicepath  in t_equipment.
-- 2. Added 5 columns in t_equipmentLog ( all path columns)
-- 3. Altered Table type Euipment Model (Added InvoicePath)
-- 4. Altered SaveAmendmentEquipment Procedure.
-- 5. Altered SaveAmendmentApproval Procedure.

-- Author : sivakatta
-- Date : 17-01-2018
-- Changes :
-- 1. Added column AddressProofType  in t_facility.
-- 2. Altered Procedure SaveFacilityDetails 
-- 3. Altered Procedure GetPCPNDTData (Getting AddressProofType in facility Details)

-- Author : sivakatta
-- Date : 18-01-2018
-- Changes :
-- 1. Added columns OwnershipType, OwnershipDocPath  in t_facility.
-- 2. Altered Procedure SaveFacilityDetails 
-- 3. Altered Procedure GetPCPNDTData (Getting OwnershipType, OwnershipDocPath in facility Details)


-- Author : Mounika
-- Date : 31-01-2018
-- Changes :
-- 1. Added a service in m_service table.








