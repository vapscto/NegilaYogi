
(function () {
    'use strict';
    angular
.module('app')
        .controller('RouteSessionScheduleController', RouteSessionScheduleController)

    RouteSessionScheduleController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function RouteSessionScheduleController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.sortKey = "trrsC_Id";   //set the sortKey to the param passed
        $scope.sortReverse = true; //if true make it false and vice versa


        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };

        $scope.arrivalchange = function (arriv) {
            // $scope.FOMST_IHalfLoginTime = maxdata;
            //$scope.FOMST_IIHalfLogoutTime = "";
            var dsttimee = arriv.trrscsL_ArrivalTime;
            $scope.sresult = $filter('date')(dsttimee, 'HH:mm:ss a');
            $scope.eresult = $filter('date')(arriv.trrscsL_DepartureTime, 'HH:mm:ss a');
            //var startTime = moment(dsttimee, "HH:mm:ss a");
            //  var endTime = moment(maxdata, "HH:mm:ss a");
            var startTime = moment($scope.sresult, "HH:mm:ss a");
            var endTime = moment($scope.eresult, "HH:mm:ss a");
            var duration = moment.duration(endTime.diff(startTime));
            var hours = parseInt(duration.asHours());
            var minutes = parseInt(duration.asMinutes()) - hours * 60;
            var finlrst = hours + ":" + minutes;

            $scope.tmin = new Date();
            $scope.tmin.setHours(hours);
            $scope.tmin.setMinutes(minutes);
            $scope.tmax = new Date();
            $scope.tmax.setHours(hours);
            $scope.tmax.setMinutes(minutes);

            $scope.ttst = new Date();
            $scope.ttst.setHours(hours);
            $scope.ttst.setMinutes(minutes);

            $scope.FOMST_FDWHrMin = $scope.ttst;
            //   $scope.FOMST_HDWHrMin = $scope.ttst;


            $scope.htmax = new Date();
            $scope.htmax.setHours(hours);
            $scope.htmax.setMinutes(minutes);




            if (arriv.trrscsL_DepartureTime >= new Date(arriv.trrscsL_ArrivalTime && new Date($scope.FOMST_IIHalfLogoutTime < arriv.trrscsL_DepartureTime) )) {
                $scope.totimemax = arriv.trrscsL_DepartureTime;
                var hh = $scope.totimemax.getHours();
                var mm = $scope.totimemax.getMinutes();
                $scope.max = arriv.trrscsL_DepartureTime;

                $scope.max.setMinutes(hh);
                $scope.max.setMinutes(mm);
            }
            else {
                arriv.trrscsL_DepartureTime = "";
            }
        }




        $scope.showlocationGrid = function (id) {
          
            var data = {
                "TRRSCS_Id": id
            }
            apiService.create("RouteSessionSchedule/showlocationGrid", data).then(function (promise) {
                if (promise != null) {
                    if (promise.getpopupdata.length > 0) {
                        $scope.getpopupdata = promise.getpopupdata;
                      
                    }
                }
            })
        }




        $scope.deffchange = function () {
            var hh1 = $scope.FOMST_IHalfLoginTime.getHours();
            var mm1 = $scope.FOMST_IHalfLoginTime.getMinutes();
            $scope.min1 = $scope.FOMST_IHalfLoginTime;
            $scope.min1.setHours(hh1);
            $scope.min1.setMinutes(mm1);

            var hh2 = $scope.FOMST_IIHalfLogoutTime.getHours();
            var mm2 = $scope.FOMST_IIHalfLogoutTime.getMinutes();
            $scope.max = $scope.FOMST_IIHalfLogoutTime;
            $scope.max.setHours(hh2);
            $scope.max.setMinutes(mm2);
        }


        $scope.depchange = function (ur) {
            var hh1 = ur.trrscsL_ArrivalTime.getHours();
            var mm1 = ur.trrscsL_ArrivalTime.getMinutes();
            $scope.min2 = ur.trrscsL_ArrivalTime;
            $scope.min2.setHours(hh1);
            $scope.min2.setMinutes(mm1);

            var hh2 = $scope.FOMST_IIHalfLogoutTime.getHours();
            var mm2 = $scope.FOMST_IIHalfLogoutTime.getMinutes();
            $scope.max1 = $scope.FOMST_IIHalfLogoutTime;
            $scope.max1.setHours(hh2);
            $scope.max1.setMinutes(mm2);
        }
        $scope.validateTomintime = function (timedata) {

            //$scope.timedis1 = false;
            //$scope.timedis2 = true;
            $scope.FOMST_IIHalfLogoutTime = "";
            $scope.totime = timedata;
            var hh = $scope.totime.getHours();
            var mm = $scope.totime.getMinutes();
            $scope.min = timedata;

            $scope.min.setHours(hh);
            $scope.min.setMinutes(mm);
            $scope.minlnc = timedata;

            $scope.minlnc.setHours(hh);
            $scope.minlnc.setMinutes(mm);
            $scope.FOMST_IHalfLogoutTime = "";
        }


        $scope.validatemax = function (maxdata) {

            // $scope.FOMST_IHalfLoginTime = maxdata;
            //$scope.FOMST_IIHalfLogoutTime = "";
            var dsttimee = $scope.FOMST_IHalfLoginTime;
            $scope.sresult = $filter('date')(dsttimee, 'HH:mm:ss a');
            $scope.eresult = $filter('date')(maxdata, 'HH:mm:ss a');
            //var startTime = moment(dsttimee, "HH:mm:ss a");
            //  var endTime = moment(maxdata, "HH:mm:ss a");
            var startTime = moment($scope.sresult, "HH:mm:ss a");
            var endTime = moment($scope.eresult, "HH:mm:ss a");
            var duration = moment.duration(endTime.diff(startTime));
            var hours = parseInt(duration.asHours());
            var minutes = parseInt(duration.asMinutes()) - hours * 60;
            var finlrst = hours + ":" + minutes;

            $scope.tmin = new Date();
            $scope.tmin.setHours(hours);
            $scope.tmin.setMinutes(minutes);
            $scope.tmax = new Date();
            $scope.tmax.setHours(hours);
            $scope.tmax.setMinutes(minutes);

            $scope.ttst = new Date();
            $scope.ttst.setHours(hours);
            $scope.ttst.setMinutes(minutes);

            $scope.FOMST_FDWHrMin = $scope.ttst;
            //   $scope.FOMST_HDWHrMin = $scope.ttst;


            $scope.htmax = new Date();
            $scope.htmax.setHours(hours);
            $scope.htmax.setMinutes(minutes);




            if (maxdata >= new Date($scope.FOMST_IHalfLoginTime)) {
                $scope.totimemax = maxdata;
                var hh = $scope.totimemax.getHours();
                var mm = $scope.totimemax.getMinutes();
                $scope.max = maxdata;

                $scope.max.setMinutes(hh);
                $scope.max.setMinutes(mm);
            }
            else {
                $scope.FOMST_IIHalfLogoutTime = "";
            }

            // $scope.FOMST_IHalfLogoutTime = "";
        }


        $scope.routechange = function () {
           
            $scope.getlocationlist_new = [];
            $scope.getlocationlist = [];
            var data = {
               "TRMR_Id": $scope.trmR_Id,
            }
            apiService.create("RouteSessionSchedule/routechange", data).then(function (promise) {
                debugger;
                if (promise.getlocationlist.length > 0) {
                    $scope.getlocationlist = promise.getlocationlist
                    angular.forEach($scope.getlocationlist, function (ss) {
                        ss.trrscsL_DepartureTime;
                        ss.trrscsL_ArrivalTime;


                    })


                }
                else {
                    swal('No Record Found');
                    
                    }
                   
               
            })

        }

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.getlocationlist, function (itm) {
                itm.checkedvalue = toggleStatus;
            });
        }



        $scope.optionToggled = function (user) {
            $scope.all = $scope.getlocationlist.every(function (itm) { return itm.checkedvalue; })
        }
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.listshow = false;
        $scope.routetypelist = [];
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("RouteSessionSchedule/getdata", pageid).
        then(function (promise) {
            $scope.getaccyear = promise.yearList;
            $scope.sessionlist = promise.sessionlist;
            $scope.getroute = promise.getroute;
            $scope.schdulelist = promise.schdulelist;

            $scope.weeklist = [];
            $scope.weeklist.push({ id: 1, type: 'SUNDAY' })
            $scope.weeklist.push({ id: 2, type: 'MONDAY' })
            $scope.weeklist.push({ id: 3, type: 'TUESDAY' })
            $scope.weeklist.push({ id: 4, type: 'WEDNESDAY' })
            $scope.weeklist.push({ id: 5, type: 'THURSDAY' })
            $scope.weeklist.push({ id: 6, type: 'FRIDAY' })
            $scope.weeklist.push({ id: 7, type: 'SATURDAY' })


            //if (promise.getdata.length > 0) {
            //    $scope.listshow = true;
            //    $scope.locationdetails = promise.getdata;
            //    $scope.presentCountgrid = $scope.locationdetails.length;
            //}
            //else {
            //    swal("No Records Found");
            //    $scope.listshow = false;
            //}
        })
        }

        $scope.submitted = false;

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //---Save--//
        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                debugger;


                //var startTime = $filter('date')(tt.trrscsL_ArrivalTime, "HH:mm");
                //var endTime = $filter('date')(tt.trrscsL_DepartureTime, "HH:mm");
                //alert(startTime)
                var weeks = [];
                var locdata = [];
                angular.forEach($scope.weeklist, function (tt) {
                    if (tt.selected == true) {
                        weeks.push({ id: tt.id,type:tt.type })
                    }
                })

                angular.forEach($scope.getlocationlist, function (tt) {
                    if (tt.checkedvalue) {

                        tt.trrscsL_ArrivalTime = $filter('date')(tt.trrscsL_ArrivalTime, "HH:mm"),
                            tt.trrscsL_DepartureTime = $filter('date')(tt.trrscsL_DepartureTime, "HH:mm"),
                        locdata.push(tt)
                    }
                })
                if (locdata.length>0) {

                var date_nn = $filter('date')($scope.TRRSC_Date, "MM/dd/yyyy");
                var data = {
                    "TRRSCS_Id": $scope.TRRSCS_Id,
                    "TRRSC_Id": $scope.TRRSC_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "TRMR_Id": $scope.trmR_Id,
                    "TRMS_Id": $scope.TRMS_Id,
                    "TRRSC_Date": new Date($scope.TRRSC_Date),
                    //"TRRSC_Date": date_nn,
                    "TRRSCS_FromTime":$filter('date')($scope.FOMST_IHalfLoginTime, "HH:mm"),
                    "TRRSCS_ToTime": $filter('date')($scope.FOMST_IIHalfLogoutTime, "HH:mm"),
                    "weekdays": weeks,
                    "loclixt": locdata,
                    //"TRRSC_Date": new Date($scope.TRRSC_Date).toDateString(),
                }
                apiService.create("RouteSessionSchedule/savedata", data).then(function (promise) {
                    debugger;
                    if (promise.message == "Save") {
                        if (promise.returnval == true) {
                            swal("Record Saved Successfull");
                        }
                        else {
                            swal("Failed To Save Record");
                        }
                    }
                    else if (promise.message == "Update") {
                        if (promise.returnval == true) {
                            swal("Record Updated Successfull");
                        }
                        else {
                            swal("Failed To Update Record");
                        }
                    }
                    else if (promise.message == "Mapped") {
                        swal("You Can Not Edit This Record It Already Mapped");
                    }
                    else if (promise.message == "Duplicate") {
                        swal("Record Already Exists");
                    }
                    $state.reload();
                    })


                }
                else
                {
                    swal('Select Atleast One Location')
                }
            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.editdata = [];
        //--edit--//
        $scope.edit = function (user) {
            $scope.editdata = [];
            var data = {
                "TRRSCS_Id": user.trrscS_Id
            }
            apiService.create("RouteSessionSchedule/edit", data).then(function (promise) {
                if (promise != null) {
                    if (promise.geteditdata.length > 0) {
                        $scope.editdata = promise.geteditdata;

                        $scope.TRRSCS_Id = promise.geteditdata[0].trrscS_Id;
                        $scope.asmaY_Id = promise.geteditdata[0].asmaY_Id;
                    $scope.trmR_Id = promise.geteditdata[0].trmR_Id;
                        $scope.TRMS_Id = promise.geteditdata[0].trmS_Id;
                        //$scope.arealist = true;
                        $scope.TRRSC_Id = promise.geteditdata[0].trrsC_Id;
                        $scope.TRRSC_Date = new Date(promise.geteditdata[0].trrsC_Date);
                        $scope.FOMST_IHalfLoginTime = moment(promise.geteditdata[0].trrscS_FromTime, 'HH:mm').format();
                        $scope.FOMST_IIHalfLogoutTime = moment(promise.geteditdata[0].trrscS_ToTime, 'HH:mm').format();
                        angular.forEach($scope.weeklist, function (ff) {
                            if (ff.type == promise.geteditdata[0].trrscS_Day) {
                                ff.selected = true;
                            }
                           
                        })
                        $scope.routechange_new(promise.geteditdata);
                       // alert('aa');
                        debugger;
                        console.log($scope.getlocationlist);
                        //angular.forEach(promise.geteditdata, function (gg) {
                        
                        //    angular.forEach($scope.getlocationlist, function (ss) {
                        //        if (gg.trmL_Id == ss.trmL_Id) {
                        //            ss.checkedvalue == true;
                        //            ss.trrscsL_DepartureTime = moment(gg.trrscsL_ArrivalTime, 'HH:mm').format();
                        //            ss.trrscsL_ArrivalTime = moment(gg.trrscsL_DepartureTime, 'HH:mm').format();
                        //        }
                           


                        //    })
                        //})

                    }
                }
            })
        }

        $scope.routechange_new = function (user) {

            $scope.getlocationlist_new = [];
            $scope.getlocationlist = [];
            var data = {
                "TRMR_Id": $scope.trmR_Id,
            }
            apiService.create("RouteSessionSchedule/routechange", data).then(function (promise) {
                debugger;
                if (promise.getlocationlist.length > 0) {
                    $scope.getlocationlist = promise.getlocationlist
                    angular.forEach(user, function (gg) {

                        angular.forEach($scope.getlocationlist, function (ss) {
                            if (gg.trmL_Id == ss.trmL_Id) {
                                ss.checkedvalue = true;
                                ss.trrscsL_DepartureTime = moment(gg.trrscsL_ArrivalTime, 'HH:mm').format();
                                ss.trrscsL_ArrivalTime = moment(gg.trrscsL_DepartureTime, 'HH:mm').format();
                            }



                        })
                    })
                }
                  


           
                else {
                    swal('No Record Found');

                }


            })

        }


        $scope.isOptionsRequired1 = function () {

            return !$scope.weeklist.some(function (sec) {
                return sec.selected;
            });
        }


        //--Active Deactive--//
        $scope.deactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.trrscS_ActiveFlg == true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {
                apiService.create("RouteSessionSchedule/activedeactive/", user).
                then(function (promise) {
                    if (promise.message != null) {
                        swal(promise.message);
                    }
                    else {
                        if (promise.returnval == true) {
                            swal(confirmmgs + " " + "Successfully");
                            $state.reload();
                        }
                        else {
                            swal(confirmmgs + " " + " Successfully");
                            $state.reload();
                        }
                    }
                })
            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
            $state.reload();
        });
        }


        $scope.deactive1 = function (user, SweetAlert) {

            var data = {
                "TRRSCSL_Id": user.trrscsL_Id,
            }
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.trrscsL_ActiveFlg == true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("RouteSessionSchedule/activedeactive/", data).
                            then(function (promise) {
                                if (promise.message != null) {
                                    swal(promise.message);
                                }
                                else {
                                    if (promise.returnval == true) {
                                        swal(confirmmgs + " " + "Successfully");
                                        $scope.showlocationGrid(user.trrscS_Id);
                                      //  $state.reload();
                                    }
                                    else {
                                        swal(confirmmgs + " " + " Successfully");
                                        $scope.showlocationGrid(user.trrscS_Id);
                                     //   $state.reload();
                                    }
                                }
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                   // $state.reload();
                });
        

        }
        //--Sorting--//     
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }


        $scope.searchValue = '';
        

        $scope.cancel = function () {
            $state.reload();
        }
    };
})();


