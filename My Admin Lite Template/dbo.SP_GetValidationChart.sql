CREATE PROCEDURE SP_GetValidationChart
AS
	SELECT  
		Val.Action,
		Count(Val.Action) as total
	From TB_M_Validation Val
	GROUP BY Val.Action;
RETURN 0