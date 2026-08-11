/* ==========================================================================
   Smart Recruitment Matching Platform - API Client & Utility Module
   ========================================================================== */

const API_BASE = window.location.origin + '/api';

const Auth = {
    getToken() {
        return localStorage.getItem('srm_token');
    },

    setToken(token) {
        localStorage.setItem('srm_token', token);
    },

    getUser() {
        const u = localStorage.getItem('srm_user');
        return u ? JSON.parse(u) : null;
    },

    setUser(user) {
        localStorage.setItem('srm_user', JSON.stringify(user));
    },

    logout() {
        localStorage.removeItem('srm_token');
        localStorage.removeItem('srm_user');
        window.location.href = '/pages/auth/login.html';
    },

    isAuthenticated() {
        return !!this.getToken();
    },

    hasRole(role) {
        const user = this.getUser();
        return user && user.role === role;
    }
};

async function apiRequest(endpoint, options = {}) {
    const token = Auth.getToken();
    const headers = {
        ...(token ? { 'Authorization': `Bearer ${token}` } : {}),
        ...options.headers
    };

    if (!(options.body instanceof FormData) && options.body && typeof options.body === 'object') {
        headers['Content-Type'] = 'application/json';
        options.body = JSON.stringify(options.body);
    }

    try {
        const response = await fetch(`${API_BASE}${endpoint}`, {
            ...options,
            headers
        });

        if (response.status === 401) {
            Auth.logout();
            throw new Error('Session expired. Please log in again.');
        }

        const data = await response.json().catch(() => null);

        if (!response.ok) {
            const errorMsg = data?.message || (typeof data === 'string' ? data : `Error ${response.status}: ${response.statusText}`);
            throw new Error(errorMsg);
        }

        return data;
    } catch (err) {
        console.error(`API Error [${endpoint}]:`, err);
        throw err;
    }
}

// UI Toast Notification helper
function showToast(message, type = 'info') {
    let container = document.querySelector('.toast-container');
    if (!container) {
        container = document.createElement('div');
        container.className = 'toast-container';
        document.body.appendChild(container);
    }

    const toast = document.createElement('div');
    toast.className = `toast toast-${type}`;
    toast.style.borderColor = type === 'success' ? 'var(--success)' : type === 'error' ? 'var(--danger)' : 'var(--primary)';
    toast.innerHTML = `<strong>${type.toUpperCase()}:</strong> ${message}`;

    container.appendChild(toast);

    setTimeout(() => {
        toast.style.opacity = '0';
        toast.style.transform = 'translateX(100%)';
        toast.style.transition = 'all 0.3s ease';
        setTimeout(() => toast.remove(), 300);
    }, 4000);
}

// Render Header Nav with user info & logout
function renderNavbar(activeNavId) {
    const user = Auth.getUser();
    const navContainer = document.getElementById('navbar-container');
    if (!navContainer) return;

    let linksHtml = '';
    if (user) {
        let dashboardUrl = '/';
        if (user.role === 'JobSeeker') dashboardUrl = '/pages/jobseeker/dashboard.html';
        else if (user.role === 'Employer') dashboardUrl = '/pages/employer/dashboard.html';
        else if (user.role === 'Administrator') dashboardUrl = '/pages/admin/dashboard.html';

        linksHtml = `
            <li class="nav-item ${activeNavId === 'dashboard' ? 'active' : ''}"><a href="${dashboardUrl}">Dashboard</a></li>
            <li class="nav-item" style="display:flex; align-items:center; gap:0.5rem;">
                <span class="badge badge-primary">${user.role}</span>
                <span style="font-weight:600; color:var(--text-main);">${user.username}</span>
            </li>
            <li><button class="btn btn-secondary btn-sm" onclick="Auth.logout()">Logout</button></li>
        `;
    } else {
        linksHtml = `
            <li class="nav-item"><a href="/">Home</a></li>
            <li class="nav-item"><a href="/pages/auth/login.html">Login</a></li>
            <li><a href="/pages/auth/register.html" class="btn btn-primary btn-sm">Register</a></li>
        `;
    }

    navContainer.innerHTML = `
        <nav class="navbar">
            <a href="/" class="brand">
                <div class="brand-icon">⚡</div>
                Smart Recruitment AI
            </a>
            <ul class="nav-links">
                ${linksHtml}
            </ul>
        </nav>
    `;
}
