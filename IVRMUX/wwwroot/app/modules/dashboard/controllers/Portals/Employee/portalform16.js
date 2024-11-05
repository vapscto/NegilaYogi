(function () {
    'use strict';
    angular
        .module('app')
        .controller('portalform16Controller', portalform16Controller)

    portalform16Controller.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache']

    function portalform16Controller($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache) {


        $scope.institutionDetails = {};
        $scope.LoadData = function () {

            $scope.showweekly = false;
            $scope.showday_d = true;
            apiService.getDATA("EmployeeForm16/getalldetails")
                .then(function (promise) {

                    if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {
                        $scope.leaveyeardropdown = promise.leaveyeardropdown;
                        //$scope.leaveyeardropdownss = promise.leaveyeardropdown[0].imfY_AssessmentYear;
                    }
                    $scope.hrmE_EmployeeFirstName = promise.empDetails[0].hrmE_EmployeeFirstName;
                    $scope.hrme_address = promise.empDetails[0].hrme_address;
                    $scope.hrmE_PFAccNo = promise.empDetails[0].hrmE_PFAccNo;
                    $scope.hrmE_FatherName = promise.empDetails[0].hrmE_FatherName;
                    $scope.hrmE_PerCity = promise.empDetails[0].hrmE_PerCity;
                    $scope.hrmE_PANCardNo = promise.empDetails[0].hrmE_PANCardNo;
                    $scope.hrmE_PerPincode = promise.empDetails[0].hrmE_PerPincode;
                    $scope.date = new Date();
                    $scope.hrmdeS_DesignationName = promise.designation[0].hrmdeS_DesignationName;


                    if (promise.institutionDetails != null) {
                        $scope.institutionDetails = promise.institutionDetails;

                        //  $('#blah').attr('src', 'https://bdcampusstrg.blob.core.windows.net/files/' + $scope.institutionDetails.mi_id + "/" + "EmployeeProfilePics" + "/" + $scope.institutionDetails.mI_Logo);

                        var instuteAddress = "";
                        if ($scope.institutionDetails.mI_Address1 != null && $scope.institutionDetails.mI_Address1 != "") {

                            instuteAddress = $scope.institutionDetails.mI_Address1;

                        }
                        if ($scope.institutionDetails.mI_Address2 != null && $scope.institutionDetails.mI_Address2 != "") {

                            instuteAddress = instuteAddress + ',' + $scope.institutionDetails.mI_Address2;

                        }

                        if ($scope.institutionDetails.mI_Address3 != null && $scope.institutionDetails.mI_Address3 != "") {

                            instuteAddress = instuteAddress + ',' + $scope.institutionDetails.mI_Address3;

                        }

                        $scope.CurrentInstuteAddress = instuteAddress;

                    }





                })

            $scope.onselectyear = function () {
                var year = $scope.IncTax.imfY_Id;
                $scope.empFinancialYear = year;
                var assessment = $scope.leaveyeardropdown[0].imfY_AssessmentYear;
                $scope.leaveyeardropdownss = assessment;

              //  var fromdate = $scope.leaveyeardropdown[0].imfY_FromDate;
              //  $scope.fromdate = fromdate.split('T00:00:00');
              //  $scope.fromdate = fromdate.split('');

               // var todate = $scope.leaveyeardropdown[0].imfY_ToDate;
               // $scope.todate = todate.split('T');
              


                angular.forEach($scope.leaveyeardropdown, function (value, key) {
                    var fdate = value.imfY_FromDate.split('T');
                    value.imfY_FromDate = fdate[0];
                    var tdate = value.imfY_ToDate.split('T');
                    value.imfY_ToDate = tdate[0];

                    var fro = value.imfY_FromDate;
                    $scope.fromdate = fro;

                    var to = value.imfY_ToDate;
                    $scope.todate = to;


                });
            };

            $scope.onselectgroup = function () {

                var year = $scope.HRMLY_LeaveYear;
                var data = {
                    "HRMLY_LeaveYear": year
                }
                var sal_list = [];

                apiService.create("EmployeeForm12BB/getdaily_data", data).
                    then(function (promise) {

                        $scope.salary_list = promise.employee_id;

                        if ($scope.salary_list.length > 0) {
                            $scope.salaryD = true;
                        } else {
                            $scope.salaryD = false;
                            swal("Employee did't have any Salary details....!!")
                        }


                        if ($scope.salary_list != null) {

                            for (var i = 0; i < $scope.salary_list.length; i++) {
                                sal_list.push({ label: $scope.salary_list[i].monthName, "y": $scope.salary_list[i].salary })
                            }
                        }

                        var chart = new CanvasJS.Chart("columnchart", {
                            animationEnabled: true,
                            animationDuration: 3000,
                            height: 350,
                            colorSet: "graphcolor",
                            axisX: {
                                labelFontSize: 13,
                            },
                            axisY: {
                                labelFontSize: 13,
                            },

                            toolTip: {
                                shared: true
                            },
                            data: [
                                {
                                    showInLegend: true,
                                    type: "pie",
                                    dataPoints: sal_list
                                }
                            ]
                        });

                        chart.render();



                        //-------------------Attendance Graph   
                        var chart = new CanvasJS.Chart("chartContainer", {
                            animationEnabled: true,
                            animationDuration: 3000,
                            height: 350,
                            colorSet: "graphcolor",
                            axisX: {
                                labelFontSize: 13,
                            },
                            axisY: {
                                labelFontSize: 13,
                            },

                            toolTip: {
                                shared: true
                            },
                            data: [
                                {
                                    type: "column",
                                    showInLegend: true,
                                    dataPoints: sal_list
                                }
                            ]
                        });
                        chart.render();

                        $scope.hreS_Year = promise.salarylist[0].hreS_Year;
                    })

            };

            $scope.showSalaryGrid = function (data) {

                $scope.HRES_Id = data.hres_id;
                apiService.getURI("EmployeeForm12BB/getsalaryalldetails/", $scope.HRES_Id)
                    .then(function (promise) {

                        $scope.SalaryE = promise.salarylistE;
                        $scope.SalaryD = promise.salarylistD;
                        $scope.TotalEarning = promise.totalEarning[0].salary;
                        $scope.totalDeduction = promise.totalDeduction[0].salary;
                        $scope.NetSalary = $scope.TotalEarning - $scope.totalDeduction
                    })
            }
        };
    };
})();