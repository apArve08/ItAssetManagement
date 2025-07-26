CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    full_name VARCHAR(100) NOT NULL,
    role VARCHAR(20) NOT NULL CHECK (role IN ('Admin', 'User')),
    is_active BOOLEAN DEFAULT true,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE assets (
    id SERIAL PRIMARY KEY,
    asset_tag VARCHAR(50) UNIQUE NOT NULL,
    name VARCHAR(100) NOT NULL,
    description TEXT,
    category VARCHAR(50) NOT NULL,
    status VARCHAR(20) NOT NULL CHECK (status IN ('Available', 'Assigned', 'Maintenance', 'Retired')),
    purchase_date DATE,
    purchase_cost DECIMAL(10, 2),
    assigned_to_user_id INTEGER REFERENCES users(id),
    location VARCHAR(100),
    serial_number VARCHAR(100),
    model VARCHAR(100),
    manufacturer VARCHAR(100),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    created_by_user_id INTEGER REFERENCES users(id)
);

CREATE TABLE activity_logs (
    id SERIAL PRIMARY KEY,
    asset_id INTEGER REFERENCES assets(id),
    user_id INTEGER REFERENCES users(id),
    action VARCHAR(50) NOT NULL,
    description TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Insert default users
INSERT INTO users (username, password_hash, email, full_name, role) VALUES
('admin', '$2a$10$YourHashedPasswordHere', 'admin@company.com', 'System Administrator', 'Admin'),
('user1', '$2a$10$YourHashedPasswordHere', 'user1@company.com', 'John Doe', 'User');

-- Create indexes for better performance
CREATE INDEX idx_assets_status ON assets(status);
CREATE INDEX idx_assets_assigned_to ON assets(assigned_to_user_id);
CREATE INDEX idx_activity_logs_asset ON activity_logs(asset_id);
CREATE INDEX idx_activity_logs_user ON activity_logs(user_id);