// src/services/authApiService.js
const API_ROOT = '/api/auth';

export default {
    login: async (email, password) => {
        const res = await fetch(`${API_ROOT}/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, password })
        });
        if (!res.ok) throw new Error('Invalid credentials');
        return res.json(); // { token, refreshToken, expiresIn }
    },
    register: async (email, password) => {
        const res = await fetch(`${API_ROOT}/register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, password })
        });
        if (!res.ok) throw new Error('Registration failed');
        return res.json();
    }
};
