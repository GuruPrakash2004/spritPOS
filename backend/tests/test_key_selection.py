"""The report's record selection keys on the number column (a clustered-key seek), not the unindexed ID (a scan of every bill)."""
from app import rpt_print


def test_id_selection_becomes_a_number_selection():
    rpt_print._NUMBERS[77] = "26/c10-0083758"
    assert rpt_print.key_selection("{TrnHdrSINV.SINVID} =", 77) == "{TrnHdrSINV.SINVNO} = '26/c10-0083758'"
    assert rpt_print.key_selection("{TRNHDRSInv.SInvID} =", 77) == "{TRNHDRSInv.SInvNO} = '26/c10-0083758'"
    assert rpt_print.key_selection("{TrnHdrPB.PBID} =", 77) == "{TrnHdrPB.PBNO} = '26/c10-0083758'"


def test_other_formulas_and_unknown_ids_are_left_as_designed():
    rpt_print._NUMBERS[78] = "x'y"   # a quote would break the formula: keep the ID
    assert rpt_print.key_selection("{TrnHdrSINV.SINVID} =", 78) == "{TrnHdrSINV.SINVID} = 78"
    assert rpt_print.key_selection("{TrnHdrSINV.SINVID} =", 999999) == "{TrnHdrSINV.SINVID} = 999999"
    assert rpt_print.key_selection("{TrnHdrSINV.Cancelled} = 'N' and {TrnHdrSINV.SINVID} =", 77) == "{TrnHdrSINV.Cancelled} = 'N' and {TrnHdrSINV.SINVID} = 77"
    assert rpt_print.key_selection("", 5) == ""
