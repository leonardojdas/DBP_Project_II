
class Company {
    #controller;
    #view;
    #id;
    #query;
    #path;

    #iconAsc;
    #iconDesc;

    #sortBy;
    #isDesc;

    constructor() {
        this.#controller = window.location.pathname.split("/")[1];
        this.#view = window.location.pathname.split("/")[2];
        this.#id = window.location.pathname.split("/")[3];
        this.#query = window.location.search;
        this.#path = window.location.pathname;

        this.#iconAsc = "&#9652;";
        this.#iconDesc = "&#9662;";

        this.#sortBy = 0;
        this.#isDesc = true;
    }

    // public methods

    Sort() {
        if (this.#isSortOrSearch()) {
            let th = document.querySelectorAll("th");

            if (this.#query === "") th[0].innerHTML += this.#iconAsc;
            else {
                this.#sortBy = (new URLSearchParams(this.#query)).get("sortBy");
                this.#isDesc = (new URLSearchParams(this.#query)).get("isDesc");

                th[this.#sortBy].innerHTML += (this.#isDesc === "true" ? this.#iconDesc : this.#iconAsc);
            }

            this.#LoadHeaderEvents(th);
        }
    };

    UpsertTitle() {
        if (this.#view === "Upsert") {
            let action = (this.#id === "0" ? "Add" : "Edit");
            document.querySelector("#title").innerHTML = action;
        }
    };

    Search() {
        if (this.#isSortOrSearch()) {
            let searchTerm = document.querySelector("#txtSearch").value;
            searchTerm = searchTerm.replace(/[^\w\s]/gi, '');
            window.location.href = `/${this.#controller}/${this.#view}/${searchTerm}`;
        }
    };

    Upsert(id) {
        if (id === undefined) id = 0;
        window.location.href = `/${this.#controller}/Upsert/${id}`;
    };

    ShowFields() {
        let salesContainer = document.querySelector("#salesContainer");
        let isHidden = salesContainer.classList.contains("hide");
        let departmentId = Number(document.querySelector("#departmentId").value);

        if (departmentId === 2) {
            if (isHidden) salesContainer.classList.remove("hide");
        } else {
            salesContainer.classList.add("hide");
        }
    };

    getEmployeeId(val) {
        if (isNaN(val)) {
            let arrayName = val.value.split(" ");
            if (arrayName.length === 2) {
                let fullName = arrayName[0] + arrayName[1];
                let employeeId = document.querySelector("#" + fullName).dataset.value;
                document.querySelector("#Employee_Id").value = employeeId;
            }
            else {
                document.querySelector("#Employee_Id").value = "";
            }
        } else {
            document.querySelector("#Employee_Id").value = val;
        }

    }

    changePrice() {
        let id = document.querySelectorAll("#price")[0].value;
        let price = document.querySelector("#price-" + id).dataset.value;
        document.querySelector("#unit-price").innerHTML = price;

        let amount = document.querySelector("#amount").value;
        this.updateTotal(amount);
    };

    updateTotal(amount) {
        let total = Number(amount) * Number(document.querySelector("#unit-price").innerHTML);
        document.querySelector("#total").innerHTML = total;
    }

    Back() {
        window.location.href = `/${this.#controller}/List`;
    };


    // private methods

    #isSortOrSearch() {
        if (this.#path !== "/" && this.#view == "List") return true;
        return false;
    }

    #LoadHeaderEvents(th) {
        th.forEach((element, index, array) => {
            if ((array.length - 1) !== index) {

                element.addEventListener("click", () => {

                    if (this.#query === "") {
                        this.#isDesc = true;
                    } else {
                        this.#isDesc = ((new URLSearchParams(this.#query)).get("isDesc") === "true" ? false : true);
                    }
                    window.location.href = `${this.#path}?sortBy=${index}&isDesc=${this.#isDesc}`;
                });
            }
        });
    }
}

// under construction, since we don't delete for this assignment
var apiHandler = {
    GET(url) {
        $.ajax({
            url: url,
            type: 'GET',
            success: function (res) {
                debugger;
            }
        });
    },
    POST(url, object) {
        object = {
            Id: 5,
            Name: "asd",
            // ....
        }

        $.ajax({
            url: url,
            type: 'GET',
            data: object,
            success: function (res) {
                debugger;
            }
        });
    },
    DELETE(url) {
        $.ajax({
            url: url,
            type: 'DELETE',
            success: function (res) {
                debugger;
            }
        });
    }
};

(function () {
    new Company().Sort();
    new Company().UpsertTitle();
})();