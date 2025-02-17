﻿CREATE PROCEDURE SP_GetIdValidation
	@Id int
AS
	SELECT 
		Val.Id, 
		Val.Action,
		F.Name as formName,
		SUP.Name as supervisorName
	From TB_M_Validation Val 
	JOIN TB_M_Form F ON Val.formId = F.Id
	JOIN TB_M_Supervisor SUP ON Val.supervisorId = SUP.Id
	where Val.Id = @Id
RETURN 0