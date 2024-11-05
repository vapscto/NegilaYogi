
(function ()
{
    var exportTable = function () {
        var link = function ($scope, elm, attr) {
            $scope.$on('export-pdf', function (e, d) {
                elm.tableExport({ type: 'pdf', escape: false });
            });
            $scope.$on('export-excel', function (e, d) {
                elm.tableExport({ type: 'excel', escape: false });
            });
            $scope.$on('export-doc', function (e, d) {
                elm.tableExport({ type: 'doc', escape: false });
            });
        }
        return {
            restrict: 'C',
            link: link
        }
    }
    
    'use strict';
    angular
.module('app').directive('exportTable', exportTable)
.controller('FeeDemandRegisterReportController', FeeDemandRegisterReportController)
    FeeDemandRegisterReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function FeeDemandRegisterReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.obj = {};
        $scope.studentType = [{ name: "Active", value: "S"}, {name: "In-Active", value: "D"}, {name: "Left", value: "L" }]
        console.log($scope.studentType);
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 1;

            apiService.getURI("FeeDemandRegister/loaddata/", pageid).
        then(function (promise) {
            $scope.customgrpList = promise.customgrpList;
            for (var i = 0; i < $scope.customgrpList.length; i++) {
                $scope.customgrpList[i].Selected1 = true;
            }
           
            $scope.groupList = promise.groupList;
            for (var i = 0; i < $scope.groupList.length; i++) {
                $scope.groupList[i].Selected2 = true;
            }
            $scope.acdYear = promise.academicYearList;
            $scope.classlist = promise.classList;
            $scope.sectionlist = promise.sectionList;
            $scope.userNamesList = promise.userNamesList;
            $scope.feeConfiguration = promise.feeconfiguration;
            $scope.admConfiguration = promise.admissinConfiguration;
            
            $scope.FMC_GroupOrTermFlg = promise.feeconfiguration[0].fmC_GroupOrTermFlg;
            if (promise.feeconfiguration[0].fmC_GroupOrTermFlg == "T") {
                $scope.termList = promise.termsList;
                for (var i = 0; i < $scope.termList.length; i++) {
                    $scope.termList[i].Selected3 = true;
                }
            }
        })
        }
       

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Cancel = function () {
            $state.reload();
        }
        $scope.isOptionsRequired1 = function () {

            return !$scope.customgrpList.some(function (options) {
                return options.Selected1;
            });
        }
        $scope.isOptionsRequired = function () {

            return !$scope.studentType.some(function (options) {
                return options.Selected;
            });
        }
        $scope.isOptionsRequired2 = function () {

            return !$scope.groupList.some(function (options) {
                return options.Selected2;
            });
        }
        $scope.isOptionsRequired3 = function () {

            return !$scope.termList.some(function (options) {
                return options.Selected3;
            });
        }
        $scope.validatetodate = function (data1) {
            $scope.toDate = data1;
            $scope.minDatet = new Date(
           $scope.toDate.getFullYear(),
           $scope.toDate.getMonth(),
          $scope.toDate.getDate());

            $scope.maxDatet = new Date(
          $scope.toDate = "",
          $scope.toDate.getMonth(),
         $scope.toDate.getDate());
        }
        $scope.selectedStudType = [];
        $scope.getStatus = function () {
            
            $scope.selectedStudType.length = 0;
            for (var i = 0; i < $scope.studentType.length; i++) {
                if ($scope.studentType[i].Selected == true) {
                    $scope.selectedStudType.push($scope.studentType[i]);
                }
            }
            if ($scope.obj.rdo == 'Indi') {
                if ($scope.YrId > 0 && $scope.clsId > 0 && $scope.secId > 0) {
                    $scope.getStudentByYrClsSec();
                }
            }
           
        }
        $scope.getYear = function () {
            $scope.YrId = $scope.obj.ASMAY_Id;
            if ($scope.obj.rdo == 'Indi') {
                if ($scope.selectedStudType.length > 0 && $scope.clsId > 0 && $scope.secId > 0) {
                    $scope.getStudentByYrClsSec();
                }
            }
        }
        $scope.getClass = function () {
            
            $scope.clsId = $scope.obj.ASMCL_Id;
            if ($scope.obj.rdo == 'Indi') {
                if ($scope.selectedStudType.length > 0 && $scope.YrId > 0 && $scope.secId > 0) {
                    $scope.getStudentByYrClsSec();
                }
            }
        }
        $scope.getStudentByYrClsSec = function () {
           
            $scope.secId = $scope.obj.ASMS_Id;
            var data = {
                "ASMCL_Id": $scope.clsId,
                "ASMAY_Id": $scope.YrId,
                "ASMS_Id": $scope.secId,
                "selectedStudType": $scope.selectedStudType,
            }
            apiService.create("FeeDemandRegister/getStudentByYrClsSec/", data).
      then(function (promise) {
         
          if (promise.studentCount > 0) {
              if ($scope.admConfiguration[0].asC_AdmNo_RegNo_RollNo_DefaultFlag == "1") {

                  for (var i = 0; i < promise.studentList.length; i++) {
                      promise.studentList[i].studentName = promise.studentList[i].studentName + ":" + promise.studentList[i].admNo;
                  }
              }
              else if ($scope.admConfiguration[0].asC_AdmNo_RegNo_RollNo_DefaultFlag == "2") {

                  for (var i = 0; i < promise.studentList.length; i++) {
                      promise.studentList[i].studentName = promise.studentList[i].regNo + ":" + promise.studentList[i].studentName;
                  }
              }
              else if ($scope.admConfiguration[0].asC_AdmNo_RegNo_RollNo_DefaultFlag == "3") {

                  for (var i = 0; i < promise.studentList.length; i++) {
                      promise.studentList[i].studentName = promise.studentList[i].admNo + ":" + promise.studentList[i].studentName;
                  }
              }
              else if ($scope.admConfiguration[0].asC_AdmNo_RegNo_RollNo_DefaultFlag == "4") {

                  for (var i = 0; i < promise.studentList.length; i++) {
                      promise.studentList[i].studentName = promise.studentList[i].studentName + ":" + promise.studentList[i].regNo;
                  }
              }
              else if ($scope.admConfiguration[0].asC_AdmNo_RegNo_RollNo_DefaultFlag == "5") {

                  for (var i = 0; i < promise.studentList.length; i++) {
                      promise.studentList[i].studentName = promise.studentList[i].studentName + ":" + promise.studentList[i].rollNo;
                  }
              }
              else if ($scope.admConfiguration[0].asC_AdmNo_RegNo_RollNo_DefaultFlag == "6") {

                  for (var i = 0; i < promise.studentList.length; i++) {
                      promise.studentList[i].studentName = promise.studentList[i].rollNo + ":" + promise.studentList[i].studentName;
                  }
              }
              $scope.studentList = promise.studentList;
          } else {
              swal("No Students Found");
          }
      })
        }
        $scope.selectedCGList = [];
        $scope.groupByCG = function () {
            $scope.selectedCGList.length = 0;
            for (var j = 0; j < $scope.customgrpList.length; j++) {
                if ($scope.customgrpList[j].Selected1 == true) {
                    $scope.selectedCGList.push($scope.customgrpList[j]);
                }
            }
            var selectedCG = {
                "selectedCGList": $scope.selectedCGList
            }
            apiService.create("FeeDemandRegister/getgroupByCG/", selectedCG).
     then(function (promise) {
         $scope.groupList = promise.groupList;
         for (var i = 0; i < $scope.groupList.length; i++) {
             $scope.groupList[i].Selected2 = true;
         }
     })
        }

        $scope.submitted = false;
        $scope.selectedGroupList = [];
        $scope.selectedTermList = [];
        $scope.selectedCustGrpList = [];
        $scope.feedemandregisterReport = [];
        $scope.getReport = function () {

            if ($scope.myForm.$valid) {
                $scope.selectedGroupList.length = 0;
                $scope.selectedTermList.length = 0;
                $scope.selectedCustGrpList.length = 0;

                if ($scope.customgrpList != "" && $scope.customgrpList != null) {
                    if ($scope.customgrpList.length > 0) {
                        for (var i = 0; i < $scope.customgrpList.length; i++) {
                            if ($scope.customgrpList[i].Selected1 == true) {
                                $scope.selectedCustGrpList.push($scope.customgrpList[i]);
                            }
                        }
                    }
                }

                if ($scope.groupList != "" && $scope.groupList != null) {
                    if ($scope.groupList.length > 0) {
                        for (var i = 0; i < $scope.groupList.length; i++) {
                            if ($scope.groupList[i].Selected2 == true) {
                                $scope.selectedGroupList.push($scope.groupList[i]);
                            }
                        }
                    }
                }
                if ($scope.termList != "" && $scope.termList != null) {
                    if ($scope.termList.length > 0) {
                        for (var i = 0; i < $scope.termList.length; i++) {
                            if ($scope.termList[i].Selected3 == true) {
                                $scope.selectedTermList.push($scope.termList[i]);
                            }
                        }
                    }
                }
                if ($scope.obj.dt == undefined) {
                    $scope.obj.dt = undefined;
                }
                else {
                    $scope.obj.dt = new Date($scope.obj.dt).toDateString();
                }
                if ($scope.obj.frmdate == undefined) {
                    $scope.obj.frmdate = undefined;
                }
                else {
                    $scope.obj.frmdate = new Date($scope.obj.frmdate).toDateString();
                }
                if ($scope.obj.todate == undefined) {
                    $scope.obj.todate = undefined;
                }
                else {
                    $scope.obj.todate = new Date($scope.obj.todate).toDateString();
                }


                if ($scope.obj.newstud == undefined) {
                    $scope.obj.newstud = 0;
                }
                else {
                    $scope.obj.newstud = 1;
                }


                var data = {
                    "type": $scope.obj.rdo,
                    "ClassSectionFlag": $scope.obj.clsec,
                    "DetailedFlag": $scope.obj.det,
                    "GrandTotalFlag": $scope.obj.grndttl,
                    "ASMAY_Id": $scope.obj.ASMAY_Id,
                    "ASMCL_Id": $scope.obj.ASMCL_Id,
                    "ASMS_Id": $scope.obj.ASMS_Id,
                    "User_Id": $scope.obj.userId,
                    "Date": $scope.obj.dt,
                    "FromDate": $scope.obj.frmdate,
                    "ToDate": $scope.obj.todate,
                    "AMST_Id": $scope.obj.AMST_Id,
                    "selectedCGList": $scope.selectedCustGrpList,
                    "selectedGroup": $scope.selectedGroupList,
                    "selectedTerm": $scope.selectedTermList,
                    "selectedStudType": $scope.selectedStudType,
                    "newstud": $scope.obj.newstud,

                }
                apiService.create("FeeDemandRegister/getReport/", data).
        then(function (promise) {
            
            if (promise.count > 0) {
                $scope.tmp = [];
                $scope.tmp.length = 0;
                $scope.studentdet = promise.studentdetails;
                $scope.colspancount = promise.installmentdetails.length;
                $scope.installmentdetails = promise.installmentdetails;
                $scope.FeeDemandRegisterInstallment = promise.feeDemandRegisterInstallment;
                $scope.FeeNames = promise.feeNames;

                for (var i = 0; i < $scope.FeeNames.length; i++) {

                    angular.forEach($scope.installmentdetails, function (value, key) {

                        var data = {
                            "columnsname": key,
                            "values": value
                        };

                        $scope.tmp.push(data);
                    });
                }
               
                $scope.grandTotalArray = [];
                for (var i = 0; i < $scope.studentdet.length; i++) {
                    $scope.grandTotal = 0;
                    for (var j = 0; j < $scope.FeeDemandRegisterInstallment.length; j++) {
                        if ($scope.studentdet[i].amsT_Id == $scope.FeeDemandRegisterInstallment[j].amsT_Id) {
                            $scope.id = $scope.studentdet[i].amsT_Id;
                            $scope.grandTotal = Number($scope.grandTotal) + Number($scope.FeeDemandRegisterInstallment[j].installmentfees);
                        }
                    }
                    $scope.grandTotalArray.push({ 'amsT_Id': $scope.id, "grandTotal": $scope.grandTotal });
                }
                $scope.TotalArray = [];
                for (var i = 0; i < $scope.studentdet.length; i++) {
                    for (var x = 0; x < $scope.FeeNames.length; x++) {
                        $scope.totalval = 0;
                        for (var j = 0; j < $scope.FeeDemandRegisterInstallment.length; j++) {
                            if ($scope.studentdet[i].amsT_Id == $scope.FeeDemandRegisterInstallment[j].amsT_Id && $scope.FeeNames[x] == $scope.FeeDemandRegisterInstallment[j].feename) {
                                $scope.id = $scope.studentdet[i].amsT_Id;
                                $scope.totalval = Number($scope.totalval) + Number($scope.FeeDemandRegisterInstallment[j].installmentfees);
                            }
                        }
                        $scope.TotalArray.push({ 'amsT_Id': $scope.id, "total": $scope.totalval });
                    }
                }
                $scope.grandArray = [];
               
                    for (var x = 0; x < $scope.FeeNames.length; x++) {
                        $scope.totalvall = 0;
                        for (var j = 0; j < $scope.FeeDemandRegisterInstallment.length; j++) {
                            if ($scope.FeeNames[x] == $scope.FeeDemandRegisterInstallment[j].feename) {
                                $scope.fname = $scope.FeeNames[x];
                                $scope.totalvall = Number($scope.totalvall) + Number($scope.FeeDemandRegisterInstallment[j].installmentfees);
                                //  $scope.rowwiseTotal = Number($scope.rowwiseTotal) + Number($scope.FeeDemandRegisterInstallment[j].installmentfees);
                               
                            }
                        }
                        $scope.grandArray.push({ 'fee': $scope.fname, "grandVal": $scope.totalvall });
                    }
                    $scope.grandInstallments = [];
                   
                    for (var i = 0; i < $scope.FeeNames.length; i++) {
                        for (var j = 0; j < $scope.installmentdetails.length; j++) {
                            $scope.totalinst = 0;
                            for (var k = 0; k < $scope.FeeDemandRegisterInstallment.length; k++) {
                                if ($scope.installmentdetails[j] == $scope.FeeDemandRegisterInstallment[k].installmentname && $scope.FeeNames[i] == $scope.FeeDemandRegisterInstallment[k].feename) {
                                    $scope.inst = $scope.installmentdetails[j];
                                    $scope.totalinst = Number($scope.totalinst) + Number($scope.FeeDemandRegisterInstallment[k].installmentfees);
                                    //  $scope.columnwiseTotal = Number($scope.columnwiseTotal) + Number($scope.FeeDemandRegisterInstallment[k].installmentfees);
                                  
                                }
                            }
                            $scope.grandInstallments.push({ 'installments': $scope.inst, "grandInstllVal": $scope.totalinst });
                        }
                    }
                    $scope.finalTotal = 0;
                    $scope.rowwiseTotal = 0;
                    $scope.columnwiseTotal = 0;
                    for (var s = 0; s < $scope.grandArray.length; s++) {
                        $scope.rowwiseTotal =Number($scope.rowwiseTotal)+Number($scope.grandArray[s].grandVal);
                    }
                    for (var t = 0; t < $scope.grandInstallments.length; t++) {
                        $scope.columnwiseTotal = Number($scope.columnwiseTotal) + Number($scope.grandInstallments[t].grandInstllVal);
                    }
                    $scope.finalTotal = Number($scope.rowwiseTotal) + Number($scope.columnwiseTotal);
                    
            }
            
            else {
                swal("No Records Found");
            }

        });
        }
            else {
                $scope.submitted = true;
            }
        }
     
        $scope.ExportToExcel = function () {
            $scope.$broadcast('export-excel', {});
        }
        $scope.radioChange = function () {
            if ($scope.obj.rdo == 'All') {
                $scope.obj.ASMS_Id = "";
                $scope.obj.AMST_Id = "";
            }
        }
    }

   
})();


