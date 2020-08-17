CREATE PROCEDURE SP_GetIdForm
	@Id int
AS
	SELECT 
		F.Id "Id", 
		F.StartDate "StartDate",
		F.EndDate "EndDate",
		F.Duration "Duration",
		DEP.Name as departmentName,
		DEP.Id as departmentId,
		SUP.Name as supervisorName,
		SUP.Id as supervisorId,
		EMP.Name as employeeName,
		EMP.Id as employeeId
		
	From TB_M_Form F 
	JOIN TB_M_Department DEP ON F.departmentId = DEP.Id
	JOIN TB_M_Supervisor SUP ON F.supervisorId = SUP.Id
	JOIN TB_M_Employee EMP ON F.employeeId= EMP.Id
	where F.Id = @Id
RETURN 0