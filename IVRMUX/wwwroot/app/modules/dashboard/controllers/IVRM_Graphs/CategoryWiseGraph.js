﻿
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
                $scope.yearmodel = true;
            
        }
                    },
            title: {
                text: "Consolidated Category Wise Statistics"
            },
            legend: {
                position: "top"
            },
            seriesDefaults: {
                type: "column"
            },
          series: [
            valueAxis: {
        });
