CREATE PROCEDURE SP_GetAllValidation
AS
	SELECT 
		Val.Id, 
		Val.Action,
		F.Id as formId,
		SUP.Name as supervisorName,
		Val.supervisorId as supervisorId
	From TB_M_Validation Val 
	JOIN TB_M_Form F ON Val.formId = F.Id
	JOIN TB_M_Supervisor SUP ON Val.supervisorId = SUP.Id
RETURN 0