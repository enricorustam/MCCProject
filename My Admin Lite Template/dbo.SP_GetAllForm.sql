CREATE PROCEDURE SP_GetAllForm
AS
	SELECT 
		F.Id, 
		F.Name,
		F.StartDate,
		F.EndDate,
		F.Duration,
		DEP.Name as departmentName,
		SUP.Name as supervisorName
	From TB_M_Form F 
	JOIN TB_M_Department DEP ON F.departmentId = DEP.Id
	JOIN TB_M_Supervisor SUP ON F.supervisorId = SUP.Id
RETURN 0