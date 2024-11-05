

//dashboard.controller("privilegesController", ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash',
//function ($rootScope, $scope, $state, $location, dashboardService, Flash) {
//    $scope.predicate = 'sno';
//    $scope.reverse = false;
//    $scope.currentPage = 1;
//    $scope.order = function (predicate) {
//        $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : false;
//        $scope.predicate = predicate;
//    };
//    $scope.students = [
//      { sno: '1', name: 'Kevin', age: 25, gender: 'boy' },
//      { sno: '2', name: 'John', age: 30, gender: 'girl' },
//      { sno: '3', name: 'Laura', age: 28, gender: 'girl' },
//      { sno: '4', name: 'Joy', age: 15, gender: 'girl' },
//      { sno: '5', name: 'Mary', age: 28, gender: 'girl' },
//      { sno: '6', name: 'Peter', age: 95, gender: 'boy' },
//      { sno: '7', name: 'Bob', age: 50, gender: 'boy' },
//      { sno: '8', name: 'Erika', age: 27, gender: 'girl' },
//      { sno: '9', name: 'Patrick', age: 40, gender: 'boy' },
//      { sno: '10', name: 'Tery', age: 60, gender: 'girl' }
//    ];
//    $scope.totalItems = $scope.students.length;
//    $scope.numPerPage = 5;
//    $scope.paginate = function (value) {
//        var begin, end, index;
//        begin = ($scope.currentPage - 1) * $scope.numPerPage;
//        end = begin + $scope.numPerPage;
//        index = $scope.students.indexOf(value);
//        return (begin <= index && index < end);
//    };


//}]);


(function () {
    'use strict';
    angular
.module('app')
.controller('MasterRolePreviledgeController', MasterRolePreviledgeController)

    MasterRolePreviledgeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', 'superCache']
    function MasterRolePreviledgeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, superCache) {
        //modal start
        //$scope.showModal = false;
        //$scope.buttonClicked = "";
        //$scope.toggleModal = function (btnClicked) {
        //    $scope.buttonClicked = btnClicked;
        //    $scope.showModal = !$scope.showModal;
        //};
        //modal end

        //infinite sample start


        //infinite sample end

        var HostName = location.host;
        $scope.disablecheck = false;
        $scope.Previous = function () {
            $window.location.href = 'http://' + HostName + '/#/app/masterroletype/';
        };

        $scope.Next = function () {
            $window.location.href = 'http://' + HostName + '/#/app/mastermenu/';
        };

        $scope.Finish = function () {
            $window.location.href = 'http://' + HostName + '/#/app/generalSettingWizardComplete/';
        };

        $scope.IsHidden = true;
        $scope.IsHidden1 = true;
        $scope.ShowHide = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;
        }

        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }

        $scope.pagesrecord = {};

        //  $scope.previouspagesrecord = {};

        $scope.adds = {};
        var stuDelRecord = {};
        $scope.page1 = "page1";
        $scope.page2 = "page2";
        $scope.page3 = "page3";
        $scope.page4 = "page4";

        $scope.reverse1 = true;
        $scope.reverse2 = true;

        $scope.filterby = function () {

            var data = {
                "prename": $scope.searchthird,
                "searchname": $scope.typethird
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("MasterRolePreviledge/1", data).
        then(function (promise) {
            $scope.thirdgrid = promise.thirdgriddata;
        })
        }


        $scope.modulefill = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10

            $scope.currentPage2 = 1;
            $scope.itemsPerPage2 = 10

            $scope.currentPage3 = 1;
            $scope.itemsPerPage3 = 10

            $scope.currentPage4 = 1;
            $scope.itemsPerPage4 = 10

            var pageid = 2;
            apiService.getURI("MasterRolePreviledge/getalldetails", pageid).
        then(function (promise) {
            $scope.roles = promise.fillroletype;
            $scope.modpages = promise.fillmodulepagesdata;
            $scope.grid1 = promise.allsaveddata;

            $scope.thirdgrid = promise.thirdgriddata;

            //var i = 0;
            //var arrayToReturn = [];
            //var name = {};

            //var arrayToReturnmodule = [];
            //var namemodule = {};

            //for (var i = 0; i < promise.thirdgriddata.length; i++) {
            //    name = promise.thirdgriddata[i].masterRoleType
            //    name["ivrmrT_Role"] = promise.thirdgriddata[i].masterRoleType["ivrmrT_Role"] + ' - ' + promise.fillmodulepagesdata[0].ivrmM_ModuleName
            //    arrayToReturn.push(name);
            //}

            //var array = $.map(arrayToReturn, function (value, index) {
            //    return [value];
            //});

            // $scope.thirdgrid = array;

            $scope.secondgrid = [];

            //$scope.totalItems = $scope.grid1.length;
            //$scope.numPerPage = 5;

            $scope.totalItemsthird = $scope.thirdgrid.length;
            $scope.numPerPagethird = 5;
        })
            $scope.order = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

            $scope.order1 = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse1 = !$scope.reverse1; //if true make it false and vice versa
            }

            $scope.order2 = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse2 = !$scope.reverse2; //if true make it false and vice versa
            }
        };

        //$scope.predicate = 'sno';
        //$scope.reverse = false;
        //$scope.currentPage = 1;
        //$scope.order = function (predicate) {
        //    $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : false;
        //    $scope.predicate = predicate;
        //};

        //$scope.paginate = function (value) {
        //    var begin, end, index;
        //    begin = ($scope.currentPage - 1) * $scope.numPerPage;
        //    end = begin + $scope.numPerPage;
        //    index = $scope.grid1.indexOf(value);
        //    return (begin <= index && index < end);
        //};

        $scope.predicatethird = 'sno';
        $scope.reversethird = false;
        $scope.currentPagethird = 1;
        $scope.order = function (predicatethird) {
            $scope.reversethird = ($scope.predicatethird === predicatethird) ? !$scope.reversethird : false;
            $scope.predicatethird = predicatethird;
        };

        $scope.paginatethird = function (value) {
            var begin, end, index;
            begin = ($scope.currentPagethird - 1) * $scope.numPerPagethird;
            end = begin + $scope.numPerPagethird;
            index = $scope.thirdgrid.indexOf(value);
            return (begin <= index && index < end);
        };


        $scope.rolechange = function () {
            for (var i = 0; i < $scope.modpages.length; i++) {
                if ($scope.modpages[i].model == true) {
                    $scope.modpages[i].model = false;
                }
            }
            $scope.firstgrid = false;
            $scope.previosgrid = false;
            $scope.gridview2 = false;
            $scope.secondgrid = [];
            $scope.previousgrid = [];
            $scope.grid1 = [];
}

        // $scope.firstgrid = true;
        $scope.getpagesname = function (modpages) {
            $scope.firstgrid = true;
            $scope.secondgrid = [];
            $scope.temptermarray = [];
            $scope.albumNameArray = [];
            angular.forEach(modpages, function (role) {
                if (role.model == true) $scope.albumNameArray.push(role);
            })

            var data = {
                "IVRMRT_Id": $scope.IVRMRT_Id,
                "IVRMMP_Id": $scope.IVRMM_Id,
                savetmpdata: $scope.albumNameArray,
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("MasterRolePreviledge/getmodulepages", data).
        then(function (promise) {

           
           
            $scope.previosgrid = true;
            if (promise.enq.length > 0) {
                $scope.previousgrid = promise.previosgriddata;
                $scope.grid1 = promise.enq;
                $scope.gridview2 = true;
                $scope.btns = true;

                //for (var i = 0; i < promise.enq.length; i++) {
                //    $scope.temptermarray.push(promise.enq[i]);
                //}

                //angular.forEach($scope.tempsecondgrid, function (qwe1) {
                //    angular.forEach($scope.temptermarray, function (qwe) {
                //        if (qwe1.ivrmmP_Id == qwe.ivrmmP_Id) {
                //            qwe.checked = true;
                //            $scope.secondgrid.push(qwe1);
                //        }
                //    })
                //});
                //$scope.grid1 = $scope.temptermarray;

            }
            else {
                $scope.grid1 = [];
                $scope.previosgrid = false;
                $scope.gridview2 = false;
                $scope.btns = false;
                swal("No Records Found!!");
            }

        })
        }

        $scope.Toggle_Addsave = function () {
            
            var toggleStatus2 = $scope.Addsaveall2;
            angular.forEach($scope.secondgrid, function (itm) {
                itm.IVRMRP_AddFlag = toggleStatus2;
            });
        }
        $scope.Toggle_addsavegrd = function () {
            
            $scope.Addsaveall2 = $scope.secondgrid.every(function (itm)

            { return itm.IVRMRP_AddFlag; });
        }

        $scope.Toggle_Deletesave = function () {
            
            var toggleStatus2 = $scope.Deletesaveall2;
            angular.forEach($scope.secondgrid, function (itm) {
                itm.IVRMRP_DeleteFlag = toggleStatus2;
            });
        }

        $scope.Toggle_Deletesavegrd = function () {
            
            $scope.Deletesaveall2 = $scope.secondgrid.every(function (itm)

            { return itm.IVRMRP_DeleteFlag; });
        }

        $scope.Toggle_Updatesave = function () {
            
            var toggleStatus2 = $scope.Updatesaveall2;
            angular.forEach($scope.secondgrid, function (itm) {
                itm.IVRMRP_UpdateFlag = toggleStatus2;
            });
        }

        $scope.Toggle_Updatesavegrd = function () {
            
            $scope.Updatesaveall2 = $scope.secondgrid.every(function (itm)

            { return itm.IVRMRP_UpdateFlag; });
        }

        $scope.Toggle_Processsave = function () {
            
            var toggleStatus2 = $scope.Processsaveall2;
            angular.forEach($scope.secondgrid, function (itm) {
                itm.IVRMRP_ProcessFlag = toggleStatus2;
            });
        }

        $scope.Toggle_Processsavegrd = function () {
            
            $scope.Processsaveall2 = $scope.secondgrid.every(function (itm)

            { return itm.IVRMRP_ProcessFlag; });
        }


        $scope.Toggle_Reportsave = function () {
            
            var toggleStatus2 = $scope.Reportsaveall2;
            angular.forEach($scope.secondgrid, function (itm) {
                itm.IVRMRP_ReportFlag = toggleStatus2;
            });
        }

        $scope.Toggle_Reportsavegrd = function () {
            
            $scope.Reportsaveall2 = $scope.secondgrid.every(function (itm)

            { return itm.IVRMRP_ReportFlag; });
        }



        // $scope.counter = 0;
        $scope.addtocart = function (SelectedStudentRecord, previousgrid, index) {
            
            $scope.gridview2 = true;
            $scope.btns = true;

            if (previousgrid != undefined) {
                var valid;
                for (var i = 0; i < previousgrid.length; i++) {
                    if (previousgrid[i].ivrmmP_Id == SelectedStudentRecord.ivrmmP_Id) {
                        swal("Already this page is saved with Current role and Module..Kindly select other pages")
                        valid = "committ";
                        SelectedStudentRecord.checked = false;
                        // $scope.disablecheck[index] = true;
                    }
                }

                if (valid != "committ") {
                    if ($scope.secondgrid.indexOf(SelectedStudentRecord) === -1) {
                        $scope.secondgrid.push(SelectedStudentRecord);
                    }
                    else {
                        $scope.secondgrid.splice($scope.secondgrid.indexOf(SelectedStudentRecord), 1);
                    }
                }
            }
            else {
                if ($scope.secondgrid.indexOf(SelectedStudentRecord) === -1) {
                    $scope.secondgrid.push(SelectedStudentRecord);
                }
                else {
                    $scope.secondgrid.splice($scope.secondgrid.indexOf(SelectedStudentRecord), 1);
                }
            }
            if ($scope.secondgrid.length > 0)
            { $scope.udisable = true }
            else { $scope.udisable = false }



            for (var i = 0; i < $scope.secondgrid.length; i++) {
                
                if ($scope.secondgrid[i].IVRMRP_ReportFlag == true) {
                    $scope.Reportsaveall2 = true;
                }
                else {
                    $scope.Reportsaveall2 = false;
                    break;

                }
            }

            for (var i = 0; i < $scope.secondgrid.length; i++) {
                
                if ($scope.secondgrid[i].IVRMRP_AddFlag == true) {
                    $scope.Addsaveall2 = true;
                }
                else {
                    $scope.Addsaveall2 = false;
                    break;

                }
            }
            for (var i = 0; i < $scope.secondgrid.length; i++) {
                
                if ($scope.secondgrid[i].IVRMRP_DeleteFlag == true) {
                    $scope.Deletesaveall2 = true;
                }
                else {
                    $scope.Deletesaveall2 = false;
                    break;

                }
            }

            for (var i = 0; i < $scope.secondgrid.length; i++) {
                
                if ($scope.secondgrid[i].IVRMRP_UpdateFlag == true) {
                    $scope.Updatesaveall2 = true;
                }
                else {
                    $scope.Updatesaveall2 = false;
                    break;

                }
            }

            for (var i = 0; i < $scope.secondgrid.length; i++) {
                
                if ($scope.secondgrid[i].IVRMRP_ProcessFlag == true) {
                    $scope.Processsaveall2 = true;
                }
                else {
                    $scope.Processsaveall2 = false;
                    break;

                }
            }
        };

        //$scope.counter = 0;
        //$scope.addtocart = function (SelectedStudentRecord, index, inc) {
        //    $scope.gridview2 = true;
        //    $scope.secondgrid[index] = SelectedStudentRecord;
        //    $scope.counter += inc;
        //};

        $scope.currenrow = {};
        $scope.deletesecondgriddata = function (index, stuDelRecord, currenrow) {



            swal({
                title: "Are you sure",
                text: "Do you want to delete record????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
      function (isConfirm) {
          if (isConfirm) {
              stuDelRecord.checked = false;

              $scope.$apply(function () {
                  $scope.secondgrid.splice($scope.secondgrid.indexOf(stuDelRecord), 1);



                  for (var i = 0; i < $scope.secondgrid.length; i++) {
                      
                      if ($scope.secondgrid[i].IVRMRP_AddFlag == true) {
                          $scope.Addsaveall2 = true;
                      }
                      else {
                          $scope.Addsaveall2 = false;
                          break;

                      }
                  }
                  for (var i = 0; i < $scope.secondgrid.length; i++) {
                      
                      if ($scope.secondgrid[i].IVRMRP_DeleteFlag == true) {
                          $scope.Deletesaveall2 = true;
                      }
                      else {
                          $scope.Deletesaveall2 = false;
                          break;

                      }
                  }

                  for (var i = 0; i < $scope.secondgrid.length; i++) {
                      
                      if ($scope.secondgrid[i].IVRMRP_UpdateFlag == true) {
                          $scope.Updatesaveall2 = true;
                      }
                      else {
                          $scope.Updatesaveall2 = false;
                          break;

                      }
                  }

                  for (var i = 0; i < $scope.secondgrid.length; i++) {
                      
                      if ($scope.secondgrid[i].IVRMRP_ProcessFlag == true) {
                          $scope.Processsaveall2 = true;
                      }
                      else {
                          $scope.Processsaveall2 = false;
                          break;

                      }
                  }
              });
              swal("Row as been removed from list Successfully");
          }
          else {
              swal("Cancelled Successfully");
          }
      });




        };

        //$scope.predicate = 'sno';
        //$scope.reverse = false;
        //$scope.currentPage = 1;
        //$scope.order = function (predicate) {
        //    $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : false;
        //    $scope.predicate = predicate;
        //};

        //$scope.paginate = function (value) {
        //    var begin, end, index;
        //    begin = ($scope.currentPage - 1) * $scope.numPerPage;
        //    end = begin + $scope.numPerPage;
        //    index = $scope.students.indexOf(value);
        //    return (begin <= index && index < end);
        //};

        var selection = {};

        $scope.SelectedStudentRecord = {};
        $scope.deleteselectedrecord = {};

        $scope.checkAll = function () {
            $scope.user.roles = angular.copy($scope.roles);
        };

        //$scope.addtocart = function (SelectedStudentRecord, index) {
        //    $scope.gridview2 = true;
        //    $scope.secondgrid[index] = SelectedStudentRecord;
        //};

        //$scope.deletesecondgriddata = function (index, deleteselectedrecord) {
        //    var newDataList = {};
        //    var i = 0;

        //    //angular.forEach(deleteselectedrecord, function () {
        //    //    if (i !== index) {
        //    //        newDataList.push($scope.deleteselectedrecord[i])
        //    //    }
        //    //    i++;
        //    //});

        //    $scope.secondgrid = newDataList;

        //};


        //$scope.currenrow = {};
        //$scope.deletesecondgriddata = function (index, stuDelRecord, currenrow) {

        //    var currentrowid = currenrow.seconduser.ivrmP_Id

        //    $scope.newDataList = {};
        //    var i = 0;

        //    angular.forEach(stuDelRecord, function () {
        //        if (i !== index) {
        //            // $scope.newDataList.push(stuDelRecord[i])
        //            $scope.newDataList[i] = $scope.secondgrid[i];
        //        }
        //        i++;
        //    });

        //    // $scope.ivrmmP_Index = index;
        //    $scope.secondgrid = $scope.newDataList;

        //};

        $scope.deletrec = function (employee, SweetAlert) {
            $scope.editEmployee = employee.ivrmrP_Id;
            var modulepageid = $scope.editEmployee
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record !!!!!!!!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("MasterRolePreviledge/deletemodpages", modulepageid).
                    then(function (promise) {

                        // $scope.thirdgrid = promise.thirdgriddata;

                        if (promise.returnvalDelete == true) {
                            swal('Record Deleted Successfully!', 'success');
                            $state.reload();
                        }
                        else {
                            swal('Record Not Deleted Successfully!');
                            $state.reload();
                        }
                    })
                }
                else {
                    swal("Record Deletion Cancelled");
                    $state.reload();
                }
            });
        }

        //$scope.myAppObjects = {};
        //$scope.checkedItems = function (myAppObjects) {
        //    var checkedItems = [];
        //    angular.forEach($scope.myAppObjects, function (appObj, arrayIndex) {
        //        angular.forEach(appObj, function (cb, key) {
        //            if (key.substring(0, 2) == "cb" && cb) {
        //                checkedItems.push(appObj.id + '-' + key)
        //            }
        //        })
        //    })
        //    return checkedItems
        //}


        $scope.clickall = function (pagesrecord) {
            console.log("dfgt");
        }

        $scope.clearid = function () {
            //$scope.firstgrid = false;
            //$scope.gridview2 = false;
            //$scope.IVRMRT_Id = "";
            //$scope.IVRMM_Id = "";

            $state.reload();
        }

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };



        $scope.savadata = function (pagesrecord, previouspagesrecord) {
            
            $scope.submitted = true;

            if ($scope.myForm.$valid) {

                var array = $.map(pagesrecord, function (value, index) {
                    return [value];
                });
                if (array.length <= 0) {
                    if (previouspagesrecord != undefined) {
                        var previousarray = $.map(previouspagesrecord, function (value, index) {
                            return [value];
                        });
                    }
                }
                //var addarray = $.map(adds, function (value, index) {
                //    return [value];
                //});

                if (previousarray != undefined) {
                    var data = {
                        "IVRMRT_Id": $scope.IVRMRT_Id,
                        "IVRMMP_Id": $scope.IVRMMP_Id,
                        // ADDDATA: addarray,
                        savetmpdata: array,
                        previoussavetmpdata: previousarray
                    }
                }
                else {
                    var data = {
                        "IVRMRT_Id": $scope.IVRMRT_Id,
                        "IVRMMP_Id": $scope.IVRMMP_Id,
                        // ADDDATA: addarray,
                        savetmpdata: array,

                    }
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("MasterRolePreviledge/", data).
                then(function (promise) {
                    
                    $scope.students = promise.fillpagesdata;
                    if (promise.returnval == "Save" || promise.returnval == "Update") {
                        // $scope.thirdgrid = promise.enq;
                        $scope.thirdgrid = promise.thirdgriddata;

                        //$scope.grid1 = "";
                        //$scope.secondgrid = "";

                        $scope.IVRMRT_Id = "";
                        $scope.IVRMM_Id = "";

                        $scope.firstgrid = false;
                        $scope.gridview2 = false;
                        if (promise.returnval == "Save") {
                            swal('Record Saved Successfully', 'success');
                            $state.reload();
                        }
                        else if (promise.returnval == "Update") {
                            swal('Record Updated Successfully', 'success');
                            $state.reload();
                        }
                    }
                    else if (promise.returnval == "NotSave" || promise.returnval == "NotUpdate") {
                        if (promise.returnval == "NotSave") {
                            swal('Record Not Saved');
                            $state.reload();
                        }
                        else if (promise.returnval == "NotUpdate") {
                            swal('Record Not Updated');
                            $state.reload();

                        }
                    }
                })
            };
        }
    }


    //angular.module('app').controller('ModalInstanceCtrl', function ($uibModalInstance, items) {
    //    var $ctrl = this;
    //    $ctrl.items = items;
    //    $ctrl.selected = {
    //        item: $ctrl.items[0]
    //    };

    //    $ctrl.ok = function () {
    //        $uibModalInstance.close($ctrl.selected.item);
    //    };

    //    $ctrl.cancel = function () {
    //        $uibModalInstance.dismiss('cancel');
    //    };
    //});


})();

//MasterRolePreviledgeController.directive('modal', function () {
//    return {
//        template: '<div class="modal fade">' +
//            '<div class="modal-dialog">' +
//              '<div class="modal-content">' +
//                '<div class="modal-header">' +
//                  '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>' +
//                  '<h4 class="modal-title">{{ buttonClicked }} clicked!!</h4>' +
//                '</div>' +
//                '<div class="modal-body" ng-transclude></div>' +
//              '</div>' +
//            '</div>' +
//          '</div>',
//        restrict: 'E',
//        transclude: true,
//        replace: true,
//        scope: true,
//        link: function postLink(scope, element, attrs) {
//            scope.$watch(attrs.visible, function (value) {
//                if (value == true)
//                    $(element).modal('show');
//                else
//                    $(element).modal('hide');
//            });

//            $(element).on('shown.bs.modal', function () {
//                scope.$apply(function () {
//                    scope.$parent[attrs.visible] = true;
//                });
//            });

//            $(element).on('hidden.bs.modal', function () {
//                scope.$apply(function () {
//                    scope.$parent[attrs.visible] = false;
//                });
//            });
//        }
//    };
//});
app.directive('scroller', function () {
    return {
        restrict: 'A',
        link: function (scope, elem, attrs) {
            elem.bind('scroll', function () {
                scope.$apply('loadMore()');
            });
        }
    };
});
