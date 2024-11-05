
(function () {
    'use strict';
    angular
.module('app')
        .controller('VahicalCertificateReportController', VahicalCertificateReportController)

    VahicalCertificateReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', 'Excel','$timeout']
    function VahicalCertificateReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, Excel, $timeout) {
        $scope.usrname = localStorage.getItem('username');
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        $scope.ddate = new Date();;
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.masterlist = false;
        $scope.sortKey = 'trdC_Id';
        $scope.sortReverse = true;
        $scope.vh = false;
        $scope.feeorder = false;
        $scope.feetext = true;
        $scope.searchValue = "";
       
        $scope.reqSelection = true;
        $scope.certdis = false;
        $scope.typelist = []
       // $scope.Cheque = false;
        $scope.loaddata = function () {
            var pageid = 2;

            $scope.typelist = [];
           

            $scope.typelist.push({ id: 8, type: 'Vehicle Insurance' })
            $scope.typelist.push({ id: 2, type: 'Vehicle Tax' })
            $scope.typelist.push({ id: 7, type: 'Vehicle Green Tax' })
            $scope.typelist.push({ id: 4, type: 'Vehicle Fitness Test' })
            $scope.typelist.push({ id: 6, type: 'Vehicle Permit' })
            $scope.typelist.push({ id: 1, type: 'Vehicle Emission Test' })
            $scope.typelist.push({ id: 3, type: 'Vehicle Speed' })
            $scope.typelist.push({ id: 5, type: 'Vehicle CeaseFire' })
            $scope.typelist.push({ id: 9, type: 'GPRS' })

            apiService.getURI("VahicalCertificateReport/getdata/", pageid).then(function (promise) {
            
                $scope.fillvahicletype = promise.fillvahicletype;
                if (promise.getloaddata.length > 0) {

                }
                else {
                    swal("No Records Found");
                    $scope.masterlist = false;
                }
                //Set From date and To date
                $scope.TRVCT_ObtainedDate = new Date();

                $scope.TRVCT_ValidTillDate = $scope.Employee.TRVCT_ObtainedDate;
                $scope.minDateTo = new Date(
                    $scope.TRVCT_ValidTillDate.getFullYear(),
                    $scope.TRVCT_ValidTillDate.getMonth(),
                    $scope.TRVCT_ValidTillDate.getDate());

            })
        }


        $scope.FormatType = "Format1";

        //setToDate
        $scope.setToDate = function (TRVCT_ObtainedDate) {

            $scope.TRVCT_ValidTillDate = TRVCT_ObtainedDate;
            $scope.minDateTo = new Date(
                $scope.TRVCT_ValidTillDate.getFullYear(),
                $scope.TRVCT_ValidTillDate.getMonth(),
                $scope.TRVCT_ValidTillDate.getDate());
            $scope.TRVCT_ValidTillDate = "";
            if ($scope.griddeatails) {
                $scope.griddeatails = false;
            }
        }

        $scope.OnchageToDate = function () {

            if ($scope.griddeatails) {
                $scope.griddeatails = false;
            }
        }




        $scope.vehicletypechange = function () {
            $scope.allstdcheck = false;
            $scope.griddeatails = false;
           
            var data = {
                "TRMVT_Id": $scope.trmvT_Id,
            }
            apiService.create("DriverChartReport/vehicletypechange", data).
                then(function (promise) {

                    $scope.fillvahicleno = promise.fillvahicleno;


                });
        }

        $scope.allstudentcheck1 = function () {

            angular.forEach($scope.typelist, function (ff) {
                if ($scope.allstdcheck1 == true) {
                    ff.checkedgrplst11 = true;

                }
                else {
                    ff.checkedgrplst11 = false;
                }


            })


        }

        $scope.firstfnc11 = function (aa) {

            $scope.allstdcheck1 = $scope.typelist.every(function (itm) { return itm.checkedgrplst11; });

        }
     
        $scope.allstudentcheck = function () {

            angular.forEach($scope.fillvahicleno, function (ff) {
                if ($scope.allstdcheck == true) {
                    ff.checkedgrplst1 = true;

                }
                else {
                    ff.checkedgrplst1 = false;
                }


            })


        }

        $scope.isOptionsRequired1 = function () {
            return !$scope.typelist.some(function (options) {
                return options.checkedgrplst11;
            });
        }
        $scope.isOptionsRequired111 = function () {
            return !$scope.fillvahicleno.some(function (options) {
                return options.checkedgrplst1;
            });
        }

        $scope.firstfnc1 = function (aa) {

            $scope.allstdcheck = $scope.fillvahicleno.every(function (itm) { return itm.checkedgrplst1; });

        }

        $scope.selecttype = function () {

            $scope.getloaddata = [];
        }






        $scope.printData = function () {
            var innerContents = '';
            if ($scope.TRVCT_CertificateType == '1' || $scope.TRVCT_CertificateType == '5' || $scope.TRVCT_CertificateType == '9') {
                 innerContents = document.getElementById("printareaId").innerHTML;
            }
            if ($scope.TRVCT_CertificateType == '3') {
                innerContents = document.getElementById("printareaId1").innerHTML;
            }
            if ($scope.TRVCT_CertificateType == '8') {
                innerContents = document.getElementById("printareaId2").innerHTML;
            }
            if ($scope.TRVCT_CertificateType == '2' || $scope.TRVCT_CertificateType == '4' || $scope.TRVCT_CertificateType == '6' || $scope.TRVCT_CertificateType == '7') {
                innerContents = document.getElementById("printareaId3").innerHTML;
            }

            if ($scope.vh == true) {
                innerContents = document.getElementById("abcd").innerHTML;
            }
            else if ($scope.vh == false) {
                innerContents = document.getElementById("abcd1").innerHTML;
            }
      
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();

        }



        $scope.clickvh = function () {
            $scope.mainlist = [];
            $scope.temparray = [];

        }




        $scope.exportToExcel = function () {
            var tableid = '';
            if ($scope.TRVCT_CertificateType == '1' || $scope.TRVCT_CertificateType == '5' || $scope.TRVCT_CertificateType == '9') {
                tableid = '#table1';
            }
            if ($scope.TRVCT_CertificateType == '3') {
                tableid = '#table11';
            }
            if ($scope.TRVCT_CertificateType == '8') {
                tableid = '#table12';
            }
            if ($scope.TRVCT_CertificateType == '2' || $scope.TRVCT_CertificateType == '4' || $scope.TRVCT_CertificateType == '6' || $scope.TRVCT_CertificateType == '7') {
                tableid = '#table13';
               
            }
            tableid = '#abcd';

            var exportHref = Excel.tableToExcel(tableid, $scope.TRVCT_CertificateType1);
            $timeout(function () { location.href = exportHref; }, 100);
        }


        $scope.getloaddata = [];

        $scope.headerlist = [
            { id: 1, name: 'Vehicle No' },
            { id: 2, name: 'Company Name' },
            { id: 3, name: 'RTO Office name' },
            { id: 4, name: 'From Date' },
            { id: 5, name: 'To Date' },
            { id: 6, name: 'Tested company' },
            { id: 7, name: 'Insurance comp. name' },
            { id: 8, name: 'Policy No' },
            { id: 9, name: 'Amount' },
            { id: 10, name: 'Cheque/DD/Reciept No' },
          
        ]

        $scope.headerlistinsurance = [
            { id: 1, name: 'Vehicle No' },
            { id: 2, name: 'Company Name' },
          
            { id: 4, name: 'From Date' },
            { id: 5, name: 'To Date' },
            { id: 7, name: 'Insurance comp. name' },
            { id: 8, name: 'Policy No' },
            { id: 9, name: 'Amount' },
            { id: 10, name: 'Cheque/DD/Reciept No' },

        ]

        $scope.headerlistemsntest = [
            { id: 1, name: 'Vehicle No' },
            { id: 2, name: 'Company Name' },
            { id: 4, name: 'From Date' },
            { id: 5, name: 'To Date' },
            { id: 6, name: 'Tested company' },
            { id: 9, name: 'Amount' },
        ]

        $scope.headerlistemsnvtax = [
            { id: 1, name: 'Vehicle No' },
            { id: 2, name: 'Company Name' },
            { id: 2, name: 'RTO Office name' },
            { id: 4, name: 'From Date' },
            { id: 5, name: 'To Date' },
            { id: 9, name: 'Amount' },
            { id: 10, name: 'Cheque/DD/Reciept No' },
        ]

        $scope.headerlistemsnspeed = [
            { id: 1, name: 'Vehicle No' },
            { id: 2, name: 'Company Name' },
            { id: 4, name: 'From Date' },
            { id: 5, name: 'To Date' },
            { id: 3, name: 'Tested company' },
        ]

        $scope.mainlist = [];
        $scope.valsstd = [];
        $scope.valsstd1 = [];
        $scope.submitted = false;
        $scope.savedata = function () {
            $scope.getloaddata = [];
            $scope.certdis = true;
            debugger;
            $scope.mainlist = [];
            $scope.valsstd = [];
            $scope.valsstd1 = [];
            $scope.vhllist = [];
            $scope.vhllist1 = [];
            if ($scope.myForm.$valid) {
                if ($scope.TRVCT_ChequeDDDate != undefined || $scope.TRVCT_ChequeDDDate != null) {
                    $scope.TRVCT_ChequeDDDate = new Date($scope.TRVCT_ChequeDDDate).toDateString();
                }
                else {
                    $scope.TRVCT_ChequeDDDate = null;
                }
              
                angular.forEach($scope.typelist, function (rrr) {

                    if (rrr.id == $scope.TRVCT_CertificateType) {
                        $scope.TRVCT_CertificateType1 = rrr.type;
                    }
                })

                //var fromdate = $scope.TRVCT_ObtainedDate == null ? "" : $filter('date')($scope.TRVCT_ObtainedDate, "dd-MM-yyyy");
               /// var tocdate = $scope.TRVCT_ValidTillDate == null ? "" : $filter('date')($scope.TRVCT_ValidTillDate, "yyyy-MM-dd");

                if ($scope.TRVCT_ObtainedDate != undefined || $scope.TRVCT_ObtainedDate != null) {
                    $scope.TRVCT_ObtainedDate = new Date($scope.TRVCT_ObtainedDate).toDateString();
                }
                if ($scope.TRVCT_ValidTillDate != undefined || $scope.TRVCT_ValidTillDate != null) {
                    $scope.TRVCT_ValidTillDate = new Date($scope.TRVCT_ValidTillDate).toDateString();
                }
                $scope.TRVCT_ModeOfPayment;

                for (var u = 0; u < $scope.fillvahicleno.length; u++) {
                    if ($scope.fillvahicleno[u].checkedgrplst1 == true) {
                        $scope.valsstd.push($scope.fillvahicleno[u]);
                    }
                }
                for (var u = 0; u < $scope.typelist.length; u++) {
                    if ($scope.typelist[u].checkedgrplst11 == true) {
                        $scope.valsstd1.push($scope.typelist[u]);
                    }
                }
                debugger;
                var data = {
                   
                    "TRVCT_ObtainedDate": $scope.TRVCT_ObtainedDate,
                    "TRVCT_ValidTillDate": $scope.TRVCT_ValidTillDate,
                    "TRVCT_CertificateType": $scope.TRVCT_CertificateType1,
                    "vhlid": $scope.valsstd,
                    "ctype": $scope.valsstd1,
                }
                apiService.create("VahicalCertificateReport/savedata/", data).then(function (promise) {

                  
                    if (promise.getloaddata != null && promise.getloaddata.length>0) {
                        $scope.getloaddata = promise.getloaddata;


                        $scope.certypelist = [];
                      
                        angular.forEach($scope.getloaddata, function (st) {
                            if ($scope.certypelist.length == 0) {
                               
                                $scope.certypelist.push({ trvcT_CertificateType: st.trvcT_CertificateType });
                            }
                            else if ($scope.certypelist.length > 0) {
                                var al_exm_cnt = 0;
                                angular.forEach($scope.certypelist, function (exm) {
                                    if (exm.trvcT_CertificateType == st.trvcT_CertificateType) {
                                        al_exm_cnt += 1;
                                    }
                                })
                                if (al_exm_cnt == 0) {
                                  
                                    $scope.certypelist.push({ trvcT_CertificateType: st.trvcT_CertificateType});
                                }
                            }
                        })

                        if ($scope.vh == false) {
                            $scope.mainlist = [];
                            angular.forEach($scope.certypelist, function (gg) {
                                $scope.vhllist = [];
                                $scope.vhllist1 = [];
                                angular.forEach($scope.getloaddata, function (ff) {

                                    if (gg.trvcT_CertificateType == ff.trvcT_CertificateType) {
                                        $scope.vhllist.push({ trvcT_Id: ff.trvcT_Id, trmV_VehicleNo: ff.trmV_VehicleNo, trvcT_CertificateType: ff.trvcT_CertificateType, trvcT_ObtainedDate: ff.trvcT_ObtainedDate, trvcT_ValidTillDate: ff.trvcT_ValidTillDate, trvcT_Remarks: ff.trvcT_Remarks, trvcT_AmountPaid: ff.trvcT_AmountPaid, trvcT_ChequeDDDate: ff.trvcT_ChequeDDDate, trvcT_ModeOfPayment: ff.trvcT_ModeOfPayment, trvcT_ChequeDDNo: ff.trvcT_ChequeDDNo, trmV_CompanyName: ff.trmV_CompanyName, trvcT_InsuranceCompany: ff.trvcT_InsuranceCompany, trvcT_PolicyNo: ff.trvcT_PolicyNo, trvcT_VECompanyName: ff.trvcT_VECompanyName, trvcT_RTOName: ff.trvcT_RTOName, trmV_Id: ff.trmV_Id });
                                        // $scope.vhllist1 = $scope.headerlist;

                                    }



                                })


                                if (gg.trvcT_CertificateType == 'Vehicle Emission Test' || gg.trvcT_CertificateType == 'Vehicle CeaseFire' || gg.trvcT_CertificateType == 'GPRS') {
                                    $scope.mainlist.push({ trvcT_CertificateType: gg.trvcT_CertificateType, vlst: $scope.vhllist, hdlist: $scope.headerlistemsntest })
                                }
                                else if (gg.trvcT_CertificateType == 'Vehicle Tax' || gg.trvcT_CertificateType == 'Vehicle Fitness Test' || gg.trvcT_CertificateType == 'Vehicle Permit' || gg.trvcT_CertificateType == 'Vehicle Green Tax') {

                                    $scope.mainlist.push({ trvcT_CertificateType: gg.trvcT_CertificateType, vlst: $scope.vhllist, hdlist: $scope.headerlistemsnvtax })
                                } else if (gg.trvcT_CertificateType == 'Vehicle Speed') {
                                    $scope.mainlist.push({ trvcT_CertificateType: gg.trvcT_CertificateType, vlst: $scope.vhllist, hdlist: $scope.headerlistemsnspeed })
                                } else if (gg.trvcT_CertificateType == 'Vehicle Insurance') {
                                    $scope.mainlist.push({ trvcT_CertificateType: gg.trvcT_CertificateType, vlst: $scope.vhllist, hdlist: $scope.headerlistinsurance })
                                }
                                //else {
                                //    $scope.mainlist.push({ trvcT_CertificateType: gg.trvcT_CertificateType, vlst: $scope.vhllist, hdlist: $scope.headerlist })
                                //}



                            })

                            var tempordlist = [];
                            angular.forEach($scope.typelist, function (rr) {
                                angular.forEach($scope.mainlist, function (yy) {
                                    if (rr.type == yy.trvcT_CertificateType) {
                                        tempordlist.push(yy);
                                    }


                                })


                            })
                            $scope.mainlist = tempordlist;



                        }
                        else {
                            $scope.grdvehiclelist = [];
                            angular.forEach($scope.getloaddata, function (st) {
                                if ($scope.grdvehiclelist.length == 0) {

                                    $scope.grdvehiclelist.push({ trmV_Id: st.trmV_Id, trmV_VehicleNo: st.trmV_VehicleNo });
                                }
                                else if ($scope.grdvehiclelist.length > 0) {
                                    var al_exm_cnt = 0;
                                    angular.forEach($scope.grdvehiclelist, function (exm) {
                                        if (exm.trmV_Id == st.trmV_Id) {
                                            al_exm_cnt += 1;
                                        }
                                    })
                                    if (al_exm_cnt == 0) {

                                        $scope.grdvehiclelist.push({ trmV_Id: st.trmV_Id, trmV_VehicleNo: st.trmV_VehicleNo });
                                    }
                                }
                            })


                            $scope.temparray = [];
                            angular.forEach($scope.grdvehiclelist, function (kk) {

                                $scope.mainlist = [];
                                angular.forEach($scope.typelist, function (zz) {
                                angular.forEach($scope.certypelist, function (gg) {


                                    if (zz.type == gg.trvcT_CertificateType) {
                                     

                                        $scope.vhllist = [];
                                        $scope.vhllist1 = [];
                                        angular.forEach($scope.getloaddata, function (ff) {

                                            if (gg.trvcT_CertificateType == ff.trvcT_CertificateType && ff.trmV_Id == kk.trmV_Id) {
                                                $scope.vhllist.push({ trvcT_Id: ff.trvcT_Id, trmV_VehicleNo: ff.trmV_VehicleNo, trvcT_CertificateType: ff.trvcT_CertificateType, trvcT_ObtainedDate: ff.trvcT_ObtainedDate, trvcT_ValidTillDate: ff.trvcT_ValidTillDate, trvcT_Remarks: ff.trvcT_Remarks, trvcT_AmountPaid: ff.trvcT_AmountPaid, trvcT_ChequeDDDate: ff.trvcT_ChequeDDDate, trvcT_ModeOfPayment: ff.trvcT_ModeOfPayment, trvcT_ChequeDDNo: ff.trvcT_ChequeDDNo, trmV_CompanyName: ff.trmV_CompanyName, trvcT_InsuranceCompany: ff.trvcT_InsuranceCompany, trvcT_PolicyNo: ff.trvcT_PolicyNo, trvcT_VECompanyName: ff.trvcT_VECompanyName, trvcT_RTOName: ff.trvcT_RTOName, trmV_Id: ff.trmV_Id });
                                                // $scope.vhllist1 = $scope.headerlist;

                                            }



                                        })


                                        if (gg.trvcT_CertificateType == 'Vehicle Emission Test' || gg.trvcT_CertificateType == 'Vehicle CeaseFire' || gg.trvcT_CertificateType == 'GPRS') {
                                            $scope.mainlist.push({ trvcT_CertificateType: gg.trvcT_CertificateType, vlst: $scope.vhllist, hdlist: $scope.headerlistemsntest })
                                        }
                                        else if (gg.trvcT_CertificateType == 'Vehicle Tax' || gg.trvcT_CertificateType == 'Vehicle Fitness Test' || gg.trvcT_CertificateType == 'Vehicle Permit' || gg.trvcT_CertificateType == 'Vehicle Green Tax') {

                                            $scope.mainlist.push({ trvcT_CertificateType: gg.trvcT_CertificateType, vlst: $scope.vhllist, hdlist: $scope.headerlistemsnvtax })
                                        } else if (gg.trvcT_CertificateType == 'Vehicle Speed') {
                                            $scope.mainlist.push({ trvcT_CertificateType: gg.trvcT_CertificateType, vlst: $scope.vhllist, hdlist: $scope.headerlistemsnspeed })
                                        } else if (gg.trvcT_CertificateType == 'Vehicle Insurance') {
                                            $scope.mainlist.push({ trvcT_CertificateType: gg.trvcT_CertificateType, vlst: $scope.vhllist, hdlist: $scope.headerlistinsurance })
                                        }
                                        //else {
                                        //    $scope.mainlist.push({ trvcT_CertificateType: gg.trvcT_CertificateType, vlst: $scope.vhllist, hdlist: $scope.headerlist })
                                        //}


                                    }
                                })
                            })
                                $scope.temparray.push({ trmV_Id: kk.trmV_Id, trmV_VehicleNo: kk.trmV_VehicleNo, clis: $scope.mainlist })
                            })





                        }


                      





                        console.log($scope.certypelist);
                        console.log($scope.mainlist);
                        console.log($scope.grdvehiclelist);
                        console.log($scope.temparray);

                    }
                  
                    else {
                        swal('No Record Found')
                    }
                    
                })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

      

      

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
        //$scope.filterValue = function (obj) {
        //    
        //    return (JSON.stringify(obj.trdC_FromKM)).indexOf($scope.searchValue) >= 0 ||
        //        (JSON.stringify(obj.trdC_ToKM)).indexOf($scope.searchValue) >= 0 ||
        //        (JSON.stringify(obj.trdC_RateKm)).indexOf($scope.searchValue) >= 0
        //}

        $scope.clear = function () {
            $state.reload();
        }
    };
})();


