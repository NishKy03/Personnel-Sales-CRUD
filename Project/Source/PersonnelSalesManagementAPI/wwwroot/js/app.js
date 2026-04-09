const personnelApiUrl = '/api/Personnels';
const salesApiUrl = '/api/Sales';
let selectedPersonnelId = null;
function openModal() {
    const PersonnelModal = document.getElementById('PersonnelModal');
    if (PersonnelModal != null) {
        PersonnelModal.style.display = 'block';
    }
}
function openEditPersonnelModal(personnelId, personnelName, personnelAge, personnelPhone) {

    document.getElementById('PersonnelId').value = personnelId;
    document.getElementById('EditPersonnelName').value = personnelName;
    document.getElementById('EditAge').value = personnelAge;
    document.getElementById('EditPhoneNumber').value = personnelPhone;

    const modal = document.getElementById('EditPersonnelModal');
    if (modal) {
        modal.style.display = 'block';
    }
}
function openSalesModal(personnelId) {
    const salesModal = document.getElementById('SalesModal');

    if (salesModal != null) {
        salesModal.style.display = 'block';
    }

    LoadSalesByPersonnel(personnelId);
}
function showSaleFormSection() {
    document.getElementById('saleFormSection').style.display = 'block';
}
function closeSaleFormSection() {
    document.getElementById('saleFormSection').style.display = 'none';

}
function closeModal() {
    const PersonnelModal = document.getElementById('PersonnelModal');
    if (PersonnelModal != null) {
        PersonnelModal.style.display = 'none';
    }

    const SalesModal = document.getElementById('SalesModal');
    if (SalesModal != null) {
        SalesModal.style.display = 'none';
    }

    const EditPersonnelModal = document.getElementById('EditPersonnelModal');
    if (EditPersonnelModal != null) {
        EditPersonnelModal.style.display = 'none';
    }
    closeSaleFormSection();
}

document.addEventListener('DOMContentLoaded', () => {
    LoadPersonnel();

    document.getElementById('PersonnelForm').addEventListener('submit', CreatePersonnel);
    document.getElementById('SaleForm').addEventListener('submit', CreateSale);

});

async function LoadPersonnel() {
    try {
        const response = await fetch(`${personnelApiUrl}`);

        if (!response.ok) {
            throw new Error('Failed to load Personnel');
        }

        const data = await response.json();
        const tbody = document.querySelector('#PersonnelTable tbody');
        tbody.innerHTML = '';

        if (!data || data.length === 0) {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td colspan="4" class="text-center">No Personnel Data Found.</td>
            `;
            tbody.appendChild(row);

        }

        data.forEach(person => {
            const row = document.createElement('tr');
            row.innerHTML = `
            <td>${person.id}</td >
            <td class="text-center"><a href="javascript:void(0)" onclick="openSalesModal(${person.id})">${person.name}</a></td>
            <td>${person.age}</td>
            <td>${person.phone}</td>
            <td class="text-center">
                <div class="btn btn-primary" onclick="openEditPersonnelModal(${person.id},'${person.name}',${person.age},'${person.phone}')">Edit</div> ||
                <div class="btn btn-danger" onclick="DeletePersonnel(${person.id})">Delete</div>
            </td>
            `;
            tbody.appendChild(row);
        });
    } catch (error) {
        console.error(error);
        alert('Error Loading Personnel Data!');
    }
}

async function CreatePersonnel(event) {
    event.preventDefault();

    const name = document.getElementById('PersonnelName').value;
    const age = parseInt(document.getElementById('Age').value);
    const phone = document.getElementById('PhoneNumber').value;

    if (name === '' && isNaN(age) && phone === '') {
        alert('All Field Must be Filled!');
        return;
    }
    if (name === '') {
        alert('Name is Required!');
        return;
    }
    if (isNaN(age)) {
        alert('Age is Required!');
        return;
    }
    if (age <= 18) {
        alert('Age Must Been 18 Above!');
        return;
    }
    if (phone === '') {
        alert('Phone Number is Required!');
        return;
    }

    const personnelDto = {
        name: name,
        age: age,
        phone: phone
    };

    try {
        const response = await fetch(`${personnelApiUrl}/CreatePersonnels`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(personnelDto)
        });

        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(errorText);
        }

        alert('Personnel Created Successfully.');
        document.getElementById('PersonnelForm').reset();
        document.getElementById('PersonnelModal').style.display = 'none';
        await LoadPersonnel();
    } catch (error) {
        console.error(error);
        alert(`Error Creating Personnel: ${error.message}`);
    }
}

async function UpdatePersonnel() {
    const id = parseInt(document.getElementById('PersonnelId').value);
    const name = document.getElementById('EditPersonnelName').value;
    const age = parseInt(document.getElementById('EditAge').value);
    const phone = document.getElementById('EditPhoneNumber').value;

    if (name === '' && isNaN(age) && phone === '') {
        alert('All Field Must be Filled!');
        return;
    }
    if (name === '') {
        alert('Name is Required!');
        return;
    }
    if (isNaN(age)) {
        alert('Age is Required!');
        return;
    }
    if (age <= 18) {
        alert('Age Must Been 18 Above!');
        return;
    }
    if (phone === '') {
        alert('Phone Number is Required!');
        return;
    }

    const updatePersonnelDto = {
        name: name,
        age: age,
        phone: phone
    };

    try {
        const response = await fetch(`${personnelApiUrl}/UpdatePersonnels/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updatePersonnelDto)
        });

        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(errorText);
        }

        alert('Personnel Updated Succesfully.');
        closeModal();
        await LoadPersonnel();

    } catch (error) {
        console.error(error);
        alert(`Error Updating Personnel: ${error.message}`);
    }
}

async function DeletePersonnel(personnelId) {
    const confirmedMessage = confirm('Are you sure you want to delete this personnel? This will also delete all related sales data.');

    if (!confirmedMessage) {
        return;
    }

    try {
        const response = await fetch(`${personnelApiUrl}/DeletePersonnels/${personnelId}`, {
            method: 'DELETE'
        });

        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(errorText);
        }

        await LoadPersonnel();

        alert('Personnel Deleted Succesfully.');
    } catch (error) {
        console.error(error);
        aler(`Error Deleting Personnel: ${error.message}`);
    }
}

async function LoadSalesByPersonnel(personnelId) {
    selectedPersonnelId = personnelId;
    try {
        const response = await fetch(`${salesApiUrl}/GetSalesByPersonnel/${personnelId}`);
        if (!response.ok) {
            throw new Error('Failed to Load Sales By Personnel!');
        }
     
        const salesDto = await response.json();
        const tbody = document.querySelector('#SalesTable tbody');
        tbody.innerHTML = '';

        if (!salesDto || salesDto.length === 0) {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td colspan="4" class="text-center">No Sales Data Found.</td>
            `;
            tbody.appendChild(row);
           
        }
        salesDto.forEach(sale => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${sale.id}</td>
                <td>${sale.sales_Amount.toFixed(2)}</td>
                <td>${sale.report_Date}</td>
                <td><button class="btn btn-danger text-center" onclick="DeleteSale(${sale.id})">Delete</button></td>
            `;
            tbody.appendChild(row);
        });
    }
    catch (error) {
        console.error(error);
        alert('Error Loading Selected Personnel Sales List!');
    }
}

async function CreateSale(event) {
    event.preventDefault();

    const sales_amount = parseFloat(document.getElementById('SaleAmount').value);
    if (isNaN(sales_amount)) {
        alert('Sales Amount Cannot be Empty!');
        return;
    }
    const saleDto = {
        sales_Amount: sales_amount,
        personnel_Id: selectedPersonnelId
    };

    try {
        const response = await fetch(`${salesApiUrl}/CreateSales`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(saleDto)
        });

        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(errorText);
        }
        alert('Sale Created Succesfully.');
        document.getElementById('SaleForm').reset();
        closeSaleFormSection();

        await LoadSalesByPersonnel(selectedPersonnelId);

    } catch (error) {
        console.error(error);
        alert(`Error Creating sale: ${error.message}`);
    }
}

async function DeleteSale(saleId) {
    const confirmedMessage = confirm('Are you sure you want to delete this sale record?');

    if (!confirmedMessage) {
        return;
    }

    try {
        const response = await fetch(`${salesApiUrl}/DeleteSales/${saleId}`, {
            method: 'DELETE'
        });

        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(errorText);
        }

        if (selectedPersonnelId !== null) {
            await LoadSalesByPersonnel(selectedPersonnelId);
        }

        alert('Sale Deleted Successfully!');
    } catch(error) {
        console.error(error);
        alert(`Error Deleting Sale:${error.message}`);
    }
}